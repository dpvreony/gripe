// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
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

            var solutionArgument = new Argument<IFileInfo>("solution")
                .AcceptExistingOnly(fileSystem)
                .SpecificFileExtensionOnly(fileSystem, ".sln");

            var msBuildInstanceNameArgument = new Argument<string?>("msbuild-instance-name");

            var rootCommand = new RootCommand("Runs Gripe analysis on a solution.")
            {
                solutionArgument
            };

            return new RootCommandAndBinderModel<CommandLineArgModelBinder>(
                rootCommand,
                new CommandLineArgModelBinder(solutionArgument, msBuildInstanceNameArgument));
        }
    }
}
