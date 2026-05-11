// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO.Abstractions;
using Whipstaff.CommandLine;

namespace Gripe.DotNetTool.CommandLine
{
    /// <summary>
    /// Binding logic for the command line arguments.
    /// </summary>
    public sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Argument<IFileInfo> _solutionArgument;
        private readonly Option<string> _msBuildInstanceNameOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="solutionArgument">Solution argument to parse and bind against.</param>
        /// <param name="msBuildInstanceNameArgument">MSBuild Instance Name argument to parse and bind against.</param>
        public CommandLineArgModelBinder(
            Argument<IFileInfo> solutionArgument,
            Option<string> msBuildInstanceNameArgument)
        {
            _solutionArgument = solutionArgument;
            _msBuildInstanceNameOption = msBuildInstanceNameArgument;
        }

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            var solution = parseResult.GetRequiredValue(_solutionArgument);
            var msBuildInstanceName = parseResult.GetValue(_msBuildInstanceNameOption);
            return new CommandLineArgModel(solution, msBuildInstanceName);
        }
    }
}