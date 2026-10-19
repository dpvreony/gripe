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
    /// Unit Tests for <see cref="UseTypeofInsteadOfTypeGetTypeAnalyzer"/>.
    /// </summary>
    public sealed class UseTypeofInsteadOfTypeGetTypeAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace ConsoleApplication1
    {
        using System.Text;

        class TypeName
        {
            public void MethodName()
            {
                System.Type.GetType(""System.String"");
            }
        }
    }";
            var expected = new DiagnosticResult(
                DiagnosticIdsHelper.UseTypeofInsteadOfSystemTypeGetType,
                "Consider usage of typeof(x) instead of System.Type.GetType.",
                DiagnosticSeverity.Warning,
                [
                    new DiagnosticResultFileLocation(
                        "Test0.cs",
                        [
                            new DiagnosticResultLineDetail(
                                10,
                                17)])]);

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new UseTypeofInsteadOfTypeGetTypeAnalyzer();
        }
    }
}
