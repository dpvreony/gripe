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
    /// Unit Tests for <see cref="DoNotUseObjectAsFieldTypeAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseObjectAsFieldTypeAnalyzerTests : AbstractAnalyzerTest<DoNotUseObjectAsFieldTypeAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseObjectAsFieldType;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsFieldTypeProof.cs", DiagnosticSeverity.Warning, 9, 25),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsFieldTypeProof.cs", DiagnosticSeverity.Warning, 12, 25),
            ];
        }
    }
}
