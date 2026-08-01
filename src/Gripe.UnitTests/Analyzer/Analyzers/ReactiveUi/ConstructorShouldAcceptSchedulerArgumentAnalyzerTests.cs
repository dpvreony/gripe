// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.ReactiveUi;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.ReactiveUi
{
    /// <summary>
    /// Unit Tests for <see cref="ConstructorShouldAcceptSchedulerArgumentAnalyzer"/>.
    /// </summary>
    public sealed class ConstructorShouldAcceptSchedulerArgumentAnalyzerTests : AbstractAnalyzerTest<ConstructorShouldAcceptSchedulerArgumentAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.ConstructorShouldAcceptSchedulerArgument;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    @"ReactiveUi\ConstructorShouldAcceptSchedulerArgumentProof.cs",
                    DiagnosticSeverity.Warning,
                    17,
                    8)
            ];
        }
    }
}
