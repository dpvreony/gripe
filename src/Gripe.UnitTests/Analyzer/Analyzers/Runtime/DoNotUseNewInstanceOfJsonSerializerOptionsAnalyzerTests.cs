// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Runtime;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Unit Tests for <see cref="DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerTests : AbstractAnalyzerTest<DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseNewInstanceOfJsonSerializerOptions;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    24,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    37,
                    16),
            ];
        }
    }
}
