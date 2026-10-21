// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.ReactiveUi;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.ReactiveUi
{
    /// <summary>
    /// Unit Tests for <see cref="NameOfReactiveObjectBasedInterfaceShouldEndWithViewModelAnalyzer"/>.
    /// </summary>
    public sealed class NameOfReactiveObjectBasedInterfaceShouldEndWithViewModelAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    namespace ReactiveUi
    {
        public interface IReactiveObject
        {
        }

        public interface Test : ReactiveUI.IReactiveObject
        {
        }
    }";

            var expected = new DiagnosticResult(
                DiagnosticIdsHelper.ReactiveObjectInterfaceShouldHaveViewModelSuffix,
                NameOfReactiveObjectBasedInterfaceShouldEndWithViewModelAnalyzer.Title,
                DiagnosticSeverity.Warning,
                [
                    new DiagnosticResultFileLocation(
                        "Test0.cs",
                        [
                            new DiagnosticResultLineDetail(
                                7,
                                25)
                        ])
                ]);

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NameOfReactiveObjectBasedInterfaceShouldEndWithViewModelAnalyzer();
        }
    }
}
