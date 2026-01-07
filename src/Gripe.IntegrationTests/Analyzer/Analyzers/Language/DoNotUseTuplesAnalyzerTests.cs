// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NetTestRegimentation.XUnit.Logging;
using Xunit;

namespace Gripe.IntegrationTests.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Integration tests for the <see cref="Gripe.Analyzer.Analyzers.Language.DoNotUseTuplesAnalyzer"/> analyzer.
    /// </summary>
    public sealed class DoNotUseTuplesAnalyzerTests : TestWithLoggingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotUseTuplesAnalyzerTests"/> class.
        /// </summary>
        /// <param name="output">Test Output Helper.</param>
        public DoNotUseTuplesAnalyzerTests(ITestOutputHelper output)
            : base(output)
        {
        }
    }
}
