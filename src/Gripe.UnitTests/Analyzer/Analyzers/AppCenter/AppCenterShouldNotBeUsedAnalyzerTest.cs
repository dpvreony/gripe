// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.AppCenter;
using Gripe.Analyzer.Analyzers.StructureMap;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.AppCenter
{
    /// <summary>
    /// Unit Tests for <see cref="AppCenterShouldNotBeUsedAnalyzer"/>.
    /// </summary>
    public class AppCenterShouldNotBeUsedAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace Microsoft.AppCenter
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
                var instance = new Microsoft.AppCenter.Test();
                instance.Method();
            }
        }
    }";

            var diagnosticResult = new DiagnosticResult(
                DiagnosticIdsHelper.MicrosoftAppCenterShouldNotBeUsed,
                "Microsoft AppCenter is end of life so should not be used.",
                DiagnosticSeverity.Warning,
                [
                new DiagnosticResultFileLocation(
                    "Test0.cs",
                    [
                        new DiagnosticResultLineDetail(21 ,31),
                        new DiagnosticResultLineDetail(22 ,16)
                        ])]);

            VerifyCSharpDiagnostic(
                test,
                diagnosticResult);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AppCenterShouldNotBeUsedAnalyzer();
        }
    }
}
