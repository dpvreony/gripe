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
    /// Unit Tests for <see cref="UseSpanForBitConverterAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForBitConverterAnalyzerTests : AbstractAnalyzerTest<UseSpanForBitConverterAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForBitConverter;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForBitConverterProof.cs";

            return
            [
                // BitConverter.ToInt32(bytes, 0) on line 26 (0-based: 25), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    25,
                    16),

                // BitConverter.GetBytes(12345) on line 39 (0-based: 38), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    38,
                    16),

                // BitConverter.ToDouble(bytes, 0) on line 53 (0-based: 52), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    52,
                    16),
            ];
        }
    }
}
