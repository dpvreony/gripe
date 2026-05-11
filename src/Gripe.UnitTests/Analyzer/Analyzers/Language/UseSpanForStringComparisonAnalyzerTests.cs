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
    /// Unit Tests for <see cref="UseSpanForStringComparisonAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForStringComparisonAnalyzerTests : AbstractAnalyzerTest<UseSpanForStringComparisonAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForStringComparison;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForStringComparisonProof.cs";

            return
            [
                // text.Substring(0, 5).Equals(comparison) on line 26 (0-based: 25), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    25,
                    16),

                // text.Substring(0, 5).Equals(comparison, StringComparison.OrdinalIgnoreCase) on line 41 (0-based: 40), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    40,
                    16),
            ];
        }
    }
}
