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
    /// Unit Tests for <see cref="UseSpanForArrayCopyAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForArrayCopyAnalyzerTests : AbstractAnalyzerTest<UseSpanForArrayCopyAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForArrayCopy;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForArrayCopyProof.cs";

            return
            [
                // Array.Copy on line 27 (0-based: 26), column position 13
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    26,
                    12),

                // source.Clone() on line 41 (0-based: 40), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    40,
                    16),
            ];
        }
    }
}
