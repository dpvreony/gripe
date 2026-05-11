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
    /// Unit Tests for <see cref="UseSpanForUnsafeMemoryAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForUnsafeMemoryAnalyzerTests : AbstractAnalyzerTest<UseSpanForUnsafeMemoryAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForUnsafeMemory;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForUnsafeMemoryProof.cs";

            return
            [
                // fixed statement on line 25 (0-based: 24), column position 13
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    24,
                    12),

                // fixed statement on line 42 (0-based: 41), column position 13
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    41,
                    12),
            ];
        }
    }
}
