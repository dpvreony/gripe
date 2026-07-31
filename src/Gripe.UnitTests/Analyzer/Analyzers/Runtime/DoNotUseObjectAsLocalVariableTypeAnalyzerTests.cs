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
    /// Unit Tests for <see cref="DoNotUseObjectAsLocalVariableTypeAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseObjectAsLocalVariableTypeAnalyzerTests : AbstractAnalyzerTest<DoNotUseObjectAsLocalVariableTypeAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseObjectAsLocalVariableType;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsLocalVariableTypeProof.cs", DiagnosticSeverity.Warning, 19, 12),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsLocalVariableTypeProof.cs", DiagnosticSeverity.Warning, 36, 12),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsFieldTypeProof.cs", DiagnosticSeverity.Warning, 8, 25),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsFieldTypeProof.cs", DiagnosticSeverity.Warning, 11, 25),
            ];
        }
    }
}
