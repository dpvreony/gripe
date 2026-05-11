// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;
using Whipstaff.CommandLine;

namespace Gripe.MSBuildBinLogStats.DotNetTool.CommandLine
{
    /// <summary>
    /// Binding logic for the command line arguments.
    /// </summary>
    public sealed class CommandLineArgModelBinder : IBinderBase<CommandLineArgModel>
    {
        private readonly Option<FileInfo> _binLogPathOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="binLogPathOption">Bin Log Path to parse and bind against.</param>
        public CommandLineArgModelBinder(Option<FileInfo> binLogPathOption)
        {
            ArgumentNullException.ThrowIfNull(binLogPathOption);

            _binLogPathOption = binLogPathOption;
        }

        /// <inheritdoc/>
        public CommandLineArgModel GetBoundValue(ParseResult parseResult)
        {
            ArgumentNullException.ThrowIfNull(parseResult);

            var binLogPath = parseResult.GetRequiredValue(_binLogPathOption);

            return new CommandLineArgModel(binLogPath!);
        }
    }
}
