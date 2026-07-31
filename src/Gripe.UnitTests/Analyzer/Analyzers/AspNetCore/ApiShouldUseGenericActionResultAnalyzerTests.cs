// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.AspNetCore;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.AspNetCore
{
    /// <summary>
    /// Unit Tests for <see cref="ApiShouldUseGenericActionResultAnalyzer"/>.
    /// </summary>
    public sealed class ApiShouldUseGenericActionResultAnalyzerTests : AbstractAnalyzerTest<ApiShouldUseGenericActionResultAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.ApiShouldUseGenericActionResult;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string proofFilePath = @"AspNetCore\ApiShouldUseGenericActionResultProof.cs";

            return
            [
                new ExpectedDiagnosticModel(proofFilePath, DiagnosticSeverity.Warning, 18, 8),
                new ExpectedDiagnosticModel(proofFilePath, DiagnosticSeverity.Warning, 32, 8),
                new ExpectedDiagnosticModel(proofFilePath, DiagnosticSeverity.Warning, 46, 8),
                new ExpectedDiagnosticModel(proofFilePath, DiagnosticSeverity.Warning, 60, 8),
            ];
        }
    }
}
