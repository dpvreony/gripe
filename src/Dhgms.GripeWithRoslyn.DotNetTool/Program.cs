// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Dhgms.GripeWithRoslyn.DotNetTool.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.CommandLine.Hosting;

namespace Dhgms.GripeWithRoslyn.DotNetTool
{
    /// <summary>
    /// Program entry point holder.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 for success, 1 for failure.</returns>
        public static Task<int> Main(string[] args)
        {
            return HostRunner.RunSimpleCliJob<
                Job,
                CommandLineArgModel,
                CommandLineArgModelBinder,
                CommandLineHandlerFactory>(
                args,
                (fileSystem, logger) => new Job(
                    new JobLogMessageActionsWrapper(
                        logger,
                        new JobLogMessageActions()),
                    fileSystem),
                new FileSystem());
        }
    }
}
