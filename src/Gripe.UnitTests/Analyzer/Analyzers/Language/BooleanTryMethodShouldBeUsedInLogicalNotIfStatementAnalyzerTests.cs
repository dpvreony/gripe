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
    /// Unit Tests for <see cref="BooleanTryMethodShouldBeUsedInLogicalNotIfStatementAnalyzer"/>.
    /// </summary>
    public sealed class BooleanTryMethodShouldBeUsedInLogicalNotIfStatementAnalyzerTests : AbstractAnalyzerTest<BooleanTryMethodShouldBeUsedInLogicalNotIfStatementAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.TryParseShouldBeUsedInLogicalNotIfStatement;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    @"Language\BooleanTryMethodShouldBeUsedInLogicalNotIfStatementProof.cs",
                    DiagnosticSeverity.Warning,
                    19,
                    12)
            ];
        }
    }
}
