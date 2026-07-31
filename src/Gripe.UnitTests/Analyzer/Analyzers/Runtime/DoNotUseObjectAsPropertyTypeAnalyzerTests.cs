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
    /// Unit Tests for <see cref="DoNotUseObjectAsPropertyTypeAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseObjectAsPropertyTypeAnalyzerTests : AbstractAnalyzerTest<DoNotUseObjectAsPropertyTypeAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseObjectAsPropertyType;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsPropertyTypeProof.cs", DiagnosticSeverity.Warning, 8, 15),
                new ExpectedDiagnosticModel(@"Runtime\DoNotUseObjectAsPropertyTypeProof.cs", DiagnosticSeverity.Warning, 11, 15),
            ];
        }
    }
}
