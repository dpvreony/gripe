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
    /// Unit tests for <see cref="AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer"/>.
    /// </summary>
    public sealed class AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzerTests : AbstractAnalyzerTest<AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfaces;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof.cs";

            return
            [
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    16,
                    30),
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Warning,
                    36,
                    30),
            ];
        }
    }
}
