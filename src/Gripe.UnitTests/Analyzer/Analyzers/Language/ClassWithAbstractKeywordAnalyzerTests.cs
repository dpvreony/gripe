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
    /// Unit Tests for <see cref="ClassWithAbstractKeywordAnalyzer"/>.
    /// </summary>
    public sealed class ClassWithAbstractKeywordAnalyzerTests : AbstractAnalyzerTest<ClassWithAbstractKeywordAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.ClassWithAbstractKeyword;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(@"Language\ClassWithAbstractKeywordProof.cs", DiagnosticSeverity.Warning, 6, 26),
                new ExpectedDiagnosticModel(@"Language\AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof.cs", DiagnosticSeverity.Warning, 16, 30),
                new ExpectedDiagnosticModel(@"Language\AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof.cs", DiagnosticSeverity.Warning, 23, 30),
                new ExpectedDiagnosticModel(@"Language\AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof.cs", DiagnosticSeverity.Warning, 36, 30),
                new ExpectedDiagnosticModel(@"Language\AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof.cs", DiagnosticSeverity.Warning, 58, 30),
            ];
        }
    }
}
