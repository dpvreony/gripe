// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Gripe.DotNetTool.CommandLine
{
    /// <summary>
    /// Factory for creating the root command and binder.
    /// </summary>
    internal sealed class CommandLineHandlerFactory : IRootCommandAndBinderFactory<CommandLineArgModelBinder>
    {
        /// <inheritdoc/>
        public RootCommandAndBinderModel<CommandLineArgModelBinder> GetRootCommandAndBinder(IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

            var solutionArgument = new Argument<IFileInfo>("solution") { CustomParser = x => CustomParser(fileSystem, x) }
                .AcceptExistingOnly(fileSystem)
                .SpecificFileExtensionsOnly(
                    fileSystem,
                    [ ".sln", ".slnx" ]);

            var msBuildInstanceNameArgument = new Option<string>("msbuild-instance-name")
            {
                Required = false
            };

            var rootCommand = new RootCommand("Runs Gripe analysis on a solution.")
            {
                solutionArgument,
                msBuildInstanceNameArgument
            };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(solutionArgument, msBuildInstanceNameArgument));
        }

        private static IFileInfo? CustomParser(IFileSystem fileSystem, ArgumentResult argumentResult)
        {
            if (argumentResult.Tokens.Count != 1)
            {
                argumentResult.AddError($"{argumentResult.Argument.Name} requires single argument");
                return null;
            }

            return fileSystem.FileInfo.New(argumentResult.Tokens[0].Value);
        }
    }
}
