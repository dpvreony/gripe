// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Runtime;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Unit Tests for the GDI+ analyzer.
    /// </summary>
    public sealed class DoNotUseGdiPlusAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace System.Drawing
    {
        public sealed class Bitmap
        {
            public Bitmap(string filename)
            {
            }

            public void Bleh()
            {
            }
        }
    }

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public void MethodName()
            {
                var bitmap = new System.Drawing.Bitmap(""somefile"");
                bitmap.Bleh();
            }
        }
    }";

            var expected = new DiagnosticResult(
                DiagnosticIdsHelper.DoNotUseGdiPlus,
                DoNotUseGdiPlusAnalyzer.Title,
                DiagnosticSeverity.Warning,
                [
                    new DiagnosticResultFileLocation(
                        "Test0.cs",
                        [
                            new DiagnosticResultLineDetail(
                                21,
                                29),
                            new DiagnosticResultLineDetail(
                                22,
                                16),
                        ])
                ]);

            VerifyCSharpDiagnostic(
                test,
                expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotUseGdiPlusAnalyzer();
        }
    }
}