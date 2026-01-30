// Copyright (c) 2015 - 2026 DPVreony and Contributors. All rights reserved.
// DPVreony and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Build.Framework;
using System.Linq;

namespace Gripe.MsBuild.NugetUnicodeChecks
{
    /// <summary>
    /// MSBuild task that checks project reference names for Unicode characters.
    /// </summary>
    public sealed class CheckProjectReferenceNamesForUnicodeTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Gets or sets the path to the project file to evaluate.
        /// </summary>
        [Required]
        public ITaskItem[]? PackageReferences { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>True if the task executed successfully; otherwise, false.</returns>
        public override bool Execute()
        {
            if (PackageReferences == null || PackageReferences.Length == 0)
            {
                Log.LogMessage("No package references to check.");
                return true;
            }

            foreach (var package in PackageReferences)
            {
                var identity = package.ItemSpec;
                var hasUnicode = identity.Any(c => c > 127);

                if (hasUnicode)
                {
                    Log.LogWarning(
                        subcategory: null,
                        warningCode: "QA0010",
                        helpKeyword: null,
                        file: null,
                        lineNumber: 0,
                        columnNumber: 0,
                        endLineNumber: 0,
                        endColumnNumber: 0,
                        message: "Package reference '{0}' contains Unicode (non-ASCII) characters which may cause compatibility issues, or indicate a unicode homograph attack.",
                        identity);
                }
            }

            return true;
        }
    }
}
