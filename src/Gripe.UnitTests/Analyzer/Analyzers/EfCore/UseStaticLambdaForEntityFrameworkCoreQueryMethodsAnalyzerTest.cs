// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.EfCore
{
    /// <summary>
    /// Unit test for <see cref="UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzer"/>.
    /// </summary>
    public sealed class UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzerTest
        : AbstractAnalyzerTest<UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseStaticLambdaForEntityFrameworkCoreQueryMethods;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    34,
                    39),
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    35,
                    40),
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    36,
                    50),
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    37,
                    40),
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    38,
                    71),
                new ExpectedDiagnosticModel(
                    "EfCore\\EfCoreStaticLambdaProof.cs",
                    DiagnosticSeverity.Warning,
                    39,
                    40),
            ];
        }
    }
}
