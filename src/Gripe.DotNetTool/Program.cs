// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Gripe.DotNetTool.CommandLine;
using Whipstaff.CommandLine.Hosting;

namespace Gripe.DotNetTool
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
            return Run(args, null, null);
        }

        /// <summary>
        /// Run wrapper, that allows for passing in custom parser and invocation configurations for testing.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="parserConfigurationFunc">Function for passing in a parser configuration to override the default behaviour of the command line parser.</param>
        /// <param name="invocationConfigurationFunc">Function for passing in a configuration to override the default invocation behaviour of the command line runner. Useful for testing and redirecting the console.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<int> Run(
            string[] args,
            Func<ParserConfiguration>? parserConfigurationFunc,
            Func<InvocationConfiguration>? invocationConfigurationFunc)
        {
            return HostRunner.RunSimpleCliJobAsync<
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
                new FileSystem(),
                parserConfigurationFunc,
                invocationConfigurationFunc);
        }
    }
}
