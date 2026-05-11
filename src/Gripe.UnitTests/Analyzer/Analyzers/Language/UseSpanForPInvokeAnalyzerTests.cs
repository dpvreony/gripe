// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Language;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Unit Tests for <see cref="UseSpanForPInvokeAnalyzer"/>.
    /// </summary>
    public sealed class UseSpanForPInvokeAnalyzerTests : AbstractAnalyzerTest<UseSpanForPInvokeAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.UseSpanForPInvoke;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\UseSpanForPInvokeProof.cs";

            return
            [
                // NativeMethods.ReadData(buffer, buffer.Length) on line 26 (0-based: 25), column position 17
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    25,
                    17),
            ];
        }
    }
}
