// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Gripe.MsBuild.NugetLicenseScanning
{
#if TBC
    public class GetPackageLicenseFiles : Task
    {
        // Input: PackageReference items (pass @(PackageReference))
        public ITaskItem[] PackageReferences { get; set; }

        // Input: Path to project.assets.json (Pass $(ProjectAssetsFile))
        public string ProjectAssetsFile { get; set; }

        // Input: Packages directory fallback (pass $(NuGetPackageRoot) or environment NUGET_PACKAGES)
        public string PackagesDirectory { get; set; }

        // Output: license file paths found
        [Output]
        public ITaskItem[] LicenseFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                var packagesDirCandidates = new List<string>();

                if (!string.IsNullOrEmpty(PackagesDirectory))
                {
                    // NuGet may expose multiple candidate paths separated by semicolon in some contexts; split defensively
                    packagesDirCandidates.AddRange(PackagesDirectory.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()));
                }

                // fallback to environment variable
                var env = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
                if (!string.IsNullOrEmpty(env))
                    packagesDirCandidates.Add(env);

                // dedupe
                packagesDirCandidates = packagesDirCandidates.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                // Map of packageId -> version discovered from assets file (if available)
                var packageVersionMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                if (!string.IsNullOrEmpty(ProjectAssetsFile) && File.Exists(ProjectAssetsFile))
                {
                    try
                    {
                        using var s = File.OpenRead(ProjectAssetsFile);
                        using var doc = JsonDocument.Parse(s);
                        // project.assets.json has a "targets" object with keys like "net6.0" then keys "Package.Id/1.2.3"
                        // We'll parse keys under "targets" and extract package id + version.
                        if (doc.RootElement.TryGetProperty("targets", out var targets))
                        {
                            foreach (var tf in targets.EnumerateObject())
                            {
                                foreach (var pkg in tf.Value.EnumerateObject())
                                {
                                    // pkg.Name like "Newtonsoft.Json/13.0.1"
                                    var name = pkg.Name;
                                    var idx = name.LastIndexOf('/');
                                    if (idx > 0)
                                    {
                                        var pkgId = name.Substring(0, idx);
                                        var ver = name.Substring(idx + 1);
                                        if (!packageVersionMap.ContainsKey(pkgId))
                                            packageVersionMap[pkgId] = ver;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogMessage(MessageImportance.Low, "GetPackageLicenseFiles: failed to parse ProjectAssetsFile: " + ex.Message);
                    }
                }

                var found = new List<ITaskItem>();

                if (PackageReferences != null)
                {
                    foreach (var pr in PackageReferences)
                    {
                        var pkgId = pr.ItemSpec; // typically the package id
                        if (string.IsNullOrEmpty(pkgId))
                            continue;

                        // Try to find version: metadata 'Version' or from assets file mapping
                        var version = pr.GetMetadata("Version");
                        if (string.IsNullOrEmpty(version) && packageVersionMap.TryGetValue(pkgId, out var mappedVersion))
                            version = mappedVersion;

                        string packageFolderCandidate = null;

                        if (!string.IsNullOrEmpty(version))
                        {
                            // try candidates
                            foreach (var baseDir in packagesDirCandidates)
                            {
                                var candidate = Path.Combine(baseDir, pkgId.ToLowerInvariant(), version);
                                if (Directory.Exists(candidate))
                                {
                                    packageFolderCandidate = candidate;
                                    break;
                                }

                                // try case-insensitive search fallback
                                try
                                {
                                    if (Directory.Exists(baseDir))
                                    {
                                        var dirs = Directory.GetDirectories(baseDir, "*", SearchOption.TopDirectoryOnly);
                                        var match = dirs.FirstOrDefault(d => string.Equals(Path.GetFileName(d), pkgId, StringComparison.OrdinalIgnoreCase));
                                        if (match != null)
                                        {
                                            var verDir = Path.Combine(match, version);
                                            if (Directory.Exists(verDir))
                                            {
                                                packageFolderCandidate = verDir;
                                                break;
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }

                        // If we still don't have the package folder, try to look for the package within project.assets.json "packageFolders" or "packageSpec"? (Already attempted mapping).
                        // If not found, try searching each packagesDir for a folder named packageId (case-insensitively) and take the highest version folder.
                        if (packageFolderCandidate == null)
                        {
                            foreach (var baseDir in packagesDirCandidates)
                            {
                                try
                                {
                                    if (!Directory.Exists(baseDir)) continue;
                                    // find folder for packageId under baseDir
                                    var matches = Directory.GetDirectories(baseDir)
                                        .Where(d => string.Equals(Path.GetFileName(d), pkgId, StringComparison.OrdinalIgnoreCase))
                                        .ToArray();
                                    if (matches.Length == 0) continue;
                                    // pick the directory with the version subfolders; optionally pick highest semantic version folder
                                    var candidate = matches[0];
                                    // if there are version subfolders, choose best version
                                    var versionDirs = Directory.GetDirectories(candidate).Select(d => Path.GetFileName(d)).ToArray();
                                    if (versionDirs.Length > 0)
                                    {
                                        // try match version if we had it
                                        if (!string.IsNullOrEmpty(version))
                                        {
                                            var vd = versionDirs.FirstOrDefault(v => v == version);
                                            if (vd != null)
                                            {
                                                packageFolderCandidate = Path.Combine(candidate, vd);
                                                break;
                                            }
                                        }
                                        // otherwise pick the highest (lexicographically) as fallback
                                        var best = versionDirs.OrderByDescending(v => v, StringComparer.OrdinalIgnoreCase).FirstOrDefault();
                                        if (best != null)
                                        {
                                            packageFolderCandidate = Path.Combine(candidate, best);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // no version subfolders, maybe package content is directly in candidate
                                        packageFolderCandidate = candidate;
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }

                        if (packageFolderCandidate == null)
                        {
                            Log.LogMessage(MessageImportance.Low, $"GetPackageLicenseFiles: Could not find package folder for {pkgId} (version {version ?? "(unknown)"}).");
                            continue;
                        }

                        // Look for likely license files inside the package folder
                        var licenseFile = FindLicenseFile(packageFolderCandidate);
                        if (licenseFile != null)
                        {
                            var ti = new TaskItem(Path.GetFullPath(licenseFile));
                            ti.SetMetadata("PackageId", pkgId);
                            ti.SetMetadata("PackageVersion", version ?? string.Empty);
                            ti.SetMetadata("FileName", Path.GetFileNameWithoutExtension(licenseFile));
                            ti.SetMetadata("Extension", Path.GetExtension(licenseFile));
                            found.Add(ti);
                            Log.LogMessage(MessageImportance.Low, $"GetPackageLicenseFiles: found license for {pkgId} at {licenseFile}");
                        }
                        else
                        {
                            Log.LogMessage(MessageImportance.Low, $"GetPackageLicenseFiles: no license file found for {pkgId} in {packageFolderCandidate}");
                        }
                    }
                }

                LicenseFiles = found.ToArray();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, true);
                return false;
            }
        }

        private string FindLicenseFile(string packageFolder)
        {
            if (!Directory.Exists(packageFolder))
                return null;

            // Common patterns and places:
            // - license, license.txt, LICENSE, LICENSE.md at root
            // - license.* anywhere in package folder
            // - but prefer files at the package root first
            var rootCandidates = new[] { "license", "license.txt", "license.md", "license.rtf", "license.html", "LICENSE", "LICENSE.txt", "LICENSE.md" };
            foreach (var name in rootCandidates)
            {
                var p = Path.Combine(packageFolder, name);
                if (File.Exists(p)) return p;
            }

            // search for license files anywhere, prefer .txt/.md
            var all = Directory.GetFiles(packageFolder, "*license*", SearchOption.AllDirectories)
                .OrderBy(f =>
                {
                    var ext = Path.GetExtension(f).ToLowerInvariant();
                    return ext == ".md" ? 0 : ext == ".txt" ? 1 : 2;
                })
                .FirstOrDefault();

            return all;
        }
    }
#endif
}
