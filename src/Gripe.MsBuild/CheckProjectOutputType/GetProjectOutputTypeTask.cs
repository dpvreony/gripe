// Copyright (c) 2015 - 2026 DPVreony and Contributors. All rights reserved.
// DPVreony and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

namespace Dhgms.QualityAssurancePack.Features.CheckProjectOutputType
{
    /// <summary>
    /// MSBuild task that loads a project file and retrieves its OutputType property.
    /// </summary>
    public sealed class GetProjectOutputTypeTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets the path to the project file to evaluate.
        /// </summary>
        [Required]
        public string? ProjectPath { get; set; }

        /// <summary>
        /// Gets or sets the target framework to use when evaluating the project.
        /// If not specified, the project's default will be used.
        /// </summary>
        public string? TargetFramework { get; set; }

        /// <summary>
        /// Gets the OutputType property value from the project.
        /// </summary>
        [Output]
        public string? OutputType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the task successfully retrieved the OutputType.
        /// </summary>
        [Output]
        public bool Success { get; private set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>True if the task executed successfully; otherwise, false.</returns>
        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(ProjectPath))
            {
                Log.LogError("ProjectPath parameter is required.");
                Success = false;
                return false;
            }

            if (!System.IO.File.Exists(ProjectPath))
            {
                Log.LogWarning($"Project file not found:  '{ProjectPath}'");
                OutputType = string.Empty;
                Success = false;
                return true; // Continue even if file not found
            }

            try
            {
                var globalProps = new Dictionary<string, string>();

                // If a specific target framework is requested, use it
                if (!string.IsNullOrWhiteSpace(TargetFramework))
                {
                    globalProps["TargetFramework"] = TargetFramework!;
                    Log.LogMessage(
                        MessageImportance.Low,
                        $"Evaluating project '{ProjectPath}' with TargetFramework='{TargetFramework}'");
                }
                else
                {
                    Log.LogMessage(
                        MessageImportance.Low,
                        $"Evaluating project '{ProjectPath}' with default target framework");
                }

                // Load the project with global properties
                using (var projectCollection = new ProjectCollection())
                {
                    var project = projectCollection.LoadProject(
                        ProjectPath!,
                        globalProps,
                        toolsVersion: null);

                    // Get the OutputType property
                    OutputType = project.GetPropertyValue("OutputType");

                    // If OutputType is empty, it defaults to "Library" in most project types
                    if (string.IsNullOrEmpty(OutputType))
                    {
                        OutputType = "Library";
                        Log.LogMessage(
                            MessageImportance.Low,
                            $"Project '{ProjectPath}' has no explicit OutputType, defaulting to 'Library'");
                    }
                    else
                    {
                        Log.LogMessage(
                            MessageImportance.Low,
                            $"Project '{ProjectPath}' has OutputType='{OutputType}'");
                    }

                    Success = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Could not evaluate OutputType for project '{ProjectPath}': {ex.Message}");
                Log.LogMessage(MessageImportance.Low, $"Exception details: {ex}");
                OutputType = string.Empty;
                Success = false;
                return true; // Continue even on error
            }
        }
    }
}
