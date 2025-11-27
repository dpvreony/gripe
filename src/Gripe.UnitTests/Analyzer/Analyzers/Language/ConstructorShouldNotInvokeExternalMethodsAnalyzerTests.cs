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
    /// Unit Tests for checking if a Constructor invokes external methods.
    /// </summary>
    public sealed class ConstructorShouldNotInvokeExternalMethodsAnalyzerTests : AbstractAnalyzerTest<ConstructorShouldNotInvokeExternalMethodsAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.ConstructorShouldNotInvokeExternalMethods;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string TupleProofFilePath = "Language\\ConstructorExternalMethodInvocationProof.cs";

            return
            [
                new ExpectedDiagnosticModel(
                    TupleProofFilePath,
                    DiagnosticSeverity.Error,
                    32,
                    16),
                new ExpectedDiagnosticModel(
                    TupleProofFilePath,
                    DiagnosticSeverity.Error,
                    38,
                    12),
                new ExpectedDiagnosticModel(
                    TupleProofFilePath,
                    DiagnosticSeverity.Error,
                    43,
                    12),
            ];
        }
    }
}
