// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO.Abstractions;
using System.Linq;

namespace Dhgms.GripeWithRoslyn.DotNetTool.CommandLine
{
    internal static class ArgumentExtensions
    {
        internal static Argument<IFileInfo> AcceptExistingOnly(this Argument<IFileInfo> argument, IFileSystem fileSystem)
        {
            argument.Validators.Add(result =>
            {
                var filePath = result.Tokens.Single().Value;
                if (!fileSystem.File.Exists(filePath))
                {
                    result.AddError($"The file '{filePath}' does not exist.");
                }
            });
            return argument;
        }
    }
}
