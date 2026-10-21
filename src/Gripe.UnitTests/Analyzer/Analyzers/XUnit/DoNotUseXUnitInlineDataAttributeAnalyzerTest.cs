// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.XUnit;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.XUnit
{
    /// <summary>
    /// Unit Tests for <see cref="DoNotUseXUnitInlineDataAttributeAnalyzerTest"/>.
    /// </summary>
    public class DoNotUseXUnitInlineDataAttributeAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace XUnit
    {
        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class InlineDataAttribute : System.Attribute
        {
        }
    }

    namespace ConsoleApplication1
    {
        using XUnit;

        public class TypeName
        {
            [InlineData()]
            public void MethodName()
            {
            }
        }
    }";
            var expected = new DiagnosticResult(
                DiagnosticIdsHelper.DoNotUseXUnitInlineDataAttribute,
                "Do not use the XUnit Attribute InlineData",
                DiagnosticSeverity.Warning,
                [
                    new DiagnosticResultFileLocation(
                        "Test0.cs",
                        [
                            new DiagnosticResultLineDetail(
                                15,
                                13)
                        ])
                ]);

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotUseXUnitInlineDataAttributeAnalyzer();
        }
    }
}
