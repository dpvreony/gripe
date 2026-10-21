// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.StructureMap;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.StructureMap
{
    /// <summary>
    /// Unit Tests for <see cref="StructureMapShouldNotBeUsedAnalyzer"/>.
    /// </summary>
    public class StructureMapShouldNotBeUsedAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace StructureMap
    {
        public class Test
        {
            public void Method()
            {
            }
        }
    }

    namespace ConsoleApplication1
    {
        using System.Text;



        class TypeName
        {
            public void MethodName()
            {
                var instance = new StructureMap.Test();
                instance.Method();
            }
        }
    }";

            var diagnostics = new DiagnosticResult(
                DiagnosticIdsHelper.StructureMapShouldNotBeUsed,
                "StructureMap is end of life so should not be used.",
                DiagnosticSeverity.Warning,
                [
                    new DiagnosticResultFileLocation(
                        "Test0.cs",
                        [
                            new DiagnosticResultLineDetail(
                                21,
                                31),
                            new DiagnosticResultLineDetail(
                                22,
                                16)
                        ])
                ]);

            VerifyCSharpDiagnostic(
                test,
                diagnostics);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new StructureMapShouldNotBeUsedAnalyzer();
        }
    }
}
