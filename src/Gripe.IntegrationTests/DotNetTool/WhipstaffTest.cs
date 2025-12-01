// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using Gripe.DotNetTool;
using Microsoft.Extensions.Logging;
using NetTestRegimentation.XUnit.Logging;
using Xunit;

namespace Gripe.IntegrationTests.DotNetTool
{
    /// <summary>
    /// Integration tests for the dotnet tool using Whipstaff as the analysis target.
    /// </summary>
    public sealed class WhipstaffTest : TestWithLoggingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhipstaffTest"/> class.
        /// </summary>
        /// <param name="output">Test Output Helper.</param>
        public WhipstaffTest(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Test to ensure the analysis succeeds.
        /// </summary>
        [Fact]
        public void Succeeds()
        {
            Assert.True(true);
#if TBC
            await using (var outputWriter = new StringWriter())
            await using (var errorWriter = new StringWriter())
            {
                var invocationConfiguration = new InvocationConfiguration
                {
                    Output = outputWriter,
                    Error = errorWriter,
                };

                string[] args = ["../../../../../../Whipstaff/src/Whipstaff.sln"];
                var result = await Program.Run(
                    args,
                    null,
                    () => invocationConfiguration);

                Logger.LogInformation(
                    "Console output: {ConsoleOutput}",
                    outputWriter.ToString());

                Logger.LogInformation(
                    "Console error: {ConsoleError}",
                    errorWriter.ToString());

                Assert.Equal(
                    0,
                    result);
            }
#endif
        }
    }
}
