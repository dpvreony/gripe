// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.MediatR;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Analyzers.MediatR
{
    /// <summary>
    /// Unit Tests for <see cref="RequestResponseTypeShouldHaveSpecificNameAnalyzer"/>.
    /// </summary>
    public sealed class RequestResponseTypeShouldHaveSpecificNameAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            const string test = @"
    namespace MediatR
    {
        public interface IRequest<TResponse>
        {
        }

        public class Response
        {
        }

        public class Test : IRequest<Response>
        {
        }
    }";

            var expected = new DiagnosticResult
            {
                Id = DiagnosticIdsHelper.MediatRResponseShouldHaveCommandResponseOrQueryResponseSuffix,
                Message = RequestResponseTypeShouldHaveSpecificNameAnalyzer.Title,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 12, 38)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new RequestResponseTypeShouldHaveSpecificNameAnalyzer();
        }
    }
}
