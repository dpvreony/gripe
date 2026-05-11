// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Strings;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Strings
{
    /// <summary>
    /// Unit Tests for <see cref="DoNotMixUnicodeBlocksAnalyzer"/>.
    /// </summary>
    public sealed class DoNotMixUnicodeBlocksAnalyzerTests : AbstractAnalyzerTest<DoNotMixUnicodeBlocksAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotMixUnicodeBlocks;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string TupleProofFilePath = "Strings\\UnicodeRangeProof.cs";

            return
            [
                new ExpectedDiagnosticModel(
                    TupleProofFilePath,
                    DiagnosticSeverity.Warning,
                    26,
                    45)
            ];
        }
    }
}
