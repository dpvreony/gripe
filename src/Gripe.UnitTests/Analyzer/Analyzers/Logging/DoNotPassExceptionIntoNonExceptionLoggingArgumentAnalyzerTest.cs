// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Logging;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Logging
{
    /// <summary>
    /// Unit tests for the <see cref="DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer"/> class.
    /// </summary>
    public sealed class DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzerTest
        : AbstractAnalyzerTest<DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotPassExceptionIntoNonExceptionLoggingArgument;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    @"Logging\DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.cs",
                    DiagnosticSeverity.Warning,
                    88,
                    42),
                new ExpectedDiagnosticModel(
                    @"Logging\DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.cs",
                    DiagnosticSeverity.Warning,
                    89,
                    53),
                new ExpectedDiagnosticModel(
                    @"Logging\DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.cs",
                    DiagnosticSeverity.Warning,
                    90,
                    44),
                new ExpectedDiagnosticModel(
                    @"Logging\DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.cs",
                    DiagnosticSeverity.Warning,
                    91,
                    42),
            ];
        }
    }
}
