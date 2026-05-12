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
    /// Unit Tests for <see cref="ClassWithAbstractKeywordAnalyzer"/>.
    /// </summary>
    public sealed class ClassWithAbstractKeywordAnalyzerTests : CodeFixVerifier
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
                Id = DiagnosticIdsHelper.ClassWithAbstractKeyword,
                Message = ClassWithAbstractKeywordAnalyzer.Title,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 4, 31)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <summary>
        /// Test to ensure classes with abstract prefix still return a warning if they have no implementations.
        /// </summary>
        [Fact]
        public void ReturnsWarningForAbstractPrefixedTypeWithoutImplementation()
        {
            const string test = @"
    namespace ConsoleApplication1
    {
        public abstract class AbstractTypeName
        {
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIdsHelper.ClassWithAbstractKeyword,
                Message = ClassWithAbstractKeywordAnalyzer.Title,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 4, 31)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <summary>
        /// Test to ensure abstract classes with implementations do not return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarningWhenAbstractClassContainsImplementation()
        {
            const string test = @"
    namespace ConsoleApplication1
    {
        public abstract class AbstractTypeName
        {
            public void DoWork()
            {
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ClassWithAbstractKeywordAnalyzer();
        }
    }
}
