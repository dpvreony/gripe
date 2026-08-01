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
    /// Unit Tests for <see cref="DoNotUseObjectAsReturnTypeAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseObjectAsReturnTypeAnalyzerTests : AbstractAnalyzerTest<DoNotUseObjectAsReturnTypeAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseObjectAsReturnType;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsReturnTypeProof.cs", DiagnosticSeverity.Warning, 18, 15),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsReturnTypeProof.cs", DiagnosticSeverity.Warning, 32, 15),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsReturnTypeProof.cs", DiagnosticSeverity.Warning, 48, 15),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsReturnTypeProof.cs", DiagnosticSeverity.Warning, 65, 15)
            ];
        }
    }
}
