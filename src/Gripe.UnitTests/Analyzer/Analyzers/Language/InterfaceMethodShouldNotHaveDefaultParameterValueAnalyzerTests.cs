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
    /// Unit tests for <see cref="InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer"/>.
    /// </summary>
    public sealed class InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzerTests : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure interface method default parameters return a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            const string test = @"
    namespace TestConsole
    {
        public interface IService
        {
            void AddItem(string name, int quantity = 2);
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIdsHelper.InterfaceMethodShouldNotHaveDefaultParameterValue,
                Message = InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer.Title,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 6, 52)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <summary>
        /// Test to ensure class method default parameters do not return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarning()
        {
            const string test = @"
    namespace TestConsole
    {
        public class Service
        {
            public void AddItem(string name, int quantity = 2)
            {
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        /// Test to ensure interface method parameters without default values do not return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarningForInterfaceMethodWithoutDefaultValue()
        {
            const string test = @"
    namespace TestConsole
    {
        public interface IService
        {
            void AddItem(string name, int quantity);
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer();
        }
    }
}
