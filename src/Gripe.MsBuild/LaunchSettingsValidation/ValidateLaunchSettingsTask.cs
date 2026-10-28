// Copyright (c) 2015 - 2026 DPVreony and Contributors. All rights reserved.
// DPVreony and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;
using System.Text.Json;
using Microsoft.Build.Framework;

namespace Gripe.MsBuild.LaunchSettingsValidation
{
    /// <summary>
    /// MSBuild task that validates the launchSettings.json file of a web project.
    /// </summary>
    public sealed class ValidateLaunchSettingsTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets the path to the launchSettings.json file.
        /// </summary>
        [Required]
        public string? LaunchSettingsFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the project is a web project. Non web projects are skipped.
        /// </summary>
        public bool IsWebProject { get; set; } = true;

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>True if the file is valid or the project is not a web project; otherwise, false.</returns>
        public override bool Execute()
        {
            if (!IsWebProject)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(LaunchSettingsFile) || !File.Exists(LaunchSettingsFile))
            {
                Log.LogError($"Missing required file: {LaunchSettingsFile}");
                return false;
            }

            try
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(LaunchSettingsFile));

                if (doc.RootElement.ValueKind != JsonValueKind.Object
                    || !doc.RootElement.TryGetProperty("profiles", out var profiles)
                    || profiles.ValueKind != JsonValueKind.Object)
                {
                    Log.LogError($"'{LaunchSettingsFile}' is missing 'profiles' section.");
                    return false;
                }

                var allValid = true;

                foreach (var profile in profiles.EnumerateObject())
                {
                    if (profile.Value.ValueKind != JsonValueKind.Object
                        || !profile.Value.TryGetProperty("applicationUrl", out var urlProp))
                    {
                        Log.LogError($"Profile '{profile.Name}' is missing 'applicationUrl'.");
                        allValid = false;
                        continue;
                    }

                    if (urlProp.ValueKind != JsonValueKind.String
                        || string.IsNullOrWhiteSpace(urlProp.GetString()))
                    {
                        Log.LogError($"Profile '{profile.Name}' has an invalid or empty 'applicationUrl'.");
                        allValid = false;
                    }
                }

                return allValid;
            }
            catch (JsonException ex)
            {
                Log.LogError($"'{LaunchSettingsFile}' is not valid JSON: {ex.Message}");
                return false;
            }
        }
    }
}
