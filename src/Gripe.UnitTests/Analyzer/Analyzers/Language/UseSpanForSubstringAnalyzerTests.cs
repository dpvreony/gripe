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
    /// Unit Tests for <see cref="UseSpanForSubstringAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForSubstringAnalyzerTests : AbstractAnalyzerTest<UseSpanForSubstringAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForSubstring;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string UseSpanForStringComparisonProof = "Language\\UseSpanForStringComparisonProof.cs";
            const string UseSpanForSubstringProofPath = "Language\\UseSpanForSubstringProof.cs";

            return
            [
                new ExpectedDiagnosticModel(
                    UseSpanForStringComparisonProof,
                    DiagnosticSeverity.Warning,
                    25,
                    16),

                new ExpectedDiagnosticModel(
                    UseSpanForStringComparisonProof,
                    DiagnosticSeverity.Warning,
                    40,
                    16),

                new ExpectedDiagnosticModel(
                    UseSpanForSubstringProofPath,
                    DiagnosticSeverity.Warning,
                    24,
                    16),

                new ExpectedDiagnosticModel(
                    UseSpanForSubstringProofPath,
                    DiagnosticSeverity.Warning,
                    38,
                    16),
            ];
        }
    }
}
