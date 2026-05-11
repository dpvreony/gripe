// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;
using Microsoft.Build.Framework;

namespace DvResearch.MsBuild
{
    /// <summary>
    /// MSBuild task that replaces suggests replacement of package references with their alternatives.
    /// </summary>
    public sealed class ReplacePackageReferenceTask : Microsoft.Build.Utilities.Task
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacePackageReferenceTask"/> class.
        /// Uses the real file system.
        /// </summary>
        public ReplacePackageReferenceTask()
            : this(new FileSystem())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacePackageReferenceTask"/> class.
        /// </summary>
        /// <param name="fileSystem">Abstracted file system instance.</param>
        public ReplacePackageReferenceTask(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Gets or sets the path to the JSON file containing the deprecated packages and their replacements.
        /// </summary>
        [Required]
        public string? JsonFilePath { get; set; }

        /// <summary>
        /// Gets or sets the package references to check.
        /// </summary>
        [Required]
        public ITaskItem[]? PackageReferences { get; set; }

        /// <inheritdoc/>
        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(JsonFilePath))
            {
                Log.LogMessage(
                    MessageImportance.Low,
                    "No JSON config file passed.");
                return true;
            }

            if (PackageReferences == null || PackageReferences.Length == 0)
            {
                Log.LogMessage(
                    MessageImportance.Low,
                    "No package references found.");
                return true;
            }

            // Load the deprecated packages and their replacements from the JSON file
            var replacementMappings = LoadDeprecatedPackages(JsonFilePath!);
            if (replacementMappings.Count < 1)
            {
                Log.LogMessage(
                    MessageImportance.Low,
                    "No package replacements in config file.");
                return true;
            }

            foreach (var packageReference in PackageReferences)
            {
                var packageName = packageReference.ItemSpec;
                if (replacementMappings.TryGetValue(
                        packageName,
                        out var alternative))
                {
                    Log.LogWarning($"The package '{packageName}' is deprecated. Consider using '{alternative}' instead.");
                }
            }

            return true;
        }

        private Dictionary<string, string> LoadDeprecatedPackages(string jsonFilePath)
        {
            var json = _fileSystem.File.ReadAllText(jsonFilePath);
            var jsonData = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(json);
            if (jsonData == null || !jsonData.TryGetValue("packages", out var packages))
            {
                return new Dictionary<string, string>();
            }

            return packages.ToDictionary(pkg => pkg["name"], pkg => pkg["alternative"]);
        }
    }
}
