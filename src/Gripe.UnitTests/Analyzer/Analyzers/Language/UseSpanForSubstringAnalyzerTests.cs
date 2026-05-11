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
            const string ProofFilePath = "Language\\UseSpanForSubstringProof.cs";

            return
            [
                // text.Substring(0, 5) on line 25 (0-based: 24), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    24,
                    17),

                // text.Substring(7) on line 39 (0-based: 38), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    38,
                    17),
            ];
        }
    }
}
