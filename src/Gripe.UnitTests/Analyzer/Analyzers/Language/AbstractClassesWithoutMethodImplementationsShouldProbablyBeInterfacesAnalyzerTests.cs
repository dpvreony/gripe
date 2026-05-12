// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Language;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Unit tests for <see cref="AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer"/>.
    /// </summary>
    public sealed class AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzerTests : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            const string test = @"
    namespace ConsoleApplication1
    {
        public abstract class TypeName
        {
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIdsHelper.AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfaces,
                Message = AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer.Title,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 4, 31),
                    },
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <summary>
        /// Test to ensure abstract classes with a concrete method do not return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarningWhenAbstractClassContainsMethodImplementation()
        {
            const string test = @"
    namespace ConsoleApplication1
    {
        public abstract class TypeName
        {
            public void DoWork()
            {
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        /// Test to ensure abstract classes with auto properties do not return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarningWhenAbstractClassContainsAutoProperty()
        {
            const string test = @"
    namespace ConsoleApplication1
    {
        public abstract class TypeName
        {
            public int Value { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer();
        }
    }
}
