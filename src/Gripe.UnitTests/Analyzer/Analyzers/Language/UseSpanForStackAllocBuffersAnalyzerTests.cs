// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Language;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Unit Tests for <see cref="UseSpanForStackAllocBuffersAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForStackAllocBuffersAnalyzerTests : AbstractAnalyzerTest<UseSpanForStackAllocBuffersAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForStackAllocBuffers;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForStackAllocBuffersProof.cs";

            return
            [
                // new byte[256] on line 24 (0-based: 23), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    23,
                    16),

                // new byte[512] on line 37 (0-based: 36), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    36,
                    16),

                // Note: new byte[1024] on line 50 should NOT trigger (above threshold)
            ];
        }
    }
}
