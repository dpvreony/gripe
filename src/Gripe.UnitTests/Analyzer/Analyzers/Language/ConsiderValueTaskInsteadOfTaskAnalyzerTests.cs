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
    /// Unit tests for <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
    /// </summary>
    public sealed class ConsiderValueTaskInsteadOfTaskAnalyzerTests : AbstractAnalyzerTest<ConsiderValueTaskInsteadOfTaskAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.ConsiderValueTaskInsteadOfTask;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            const string ProofFilePath = "Language\\ConsiderValueTaskInsteadOfTaskProof.cs";

            return
            [
                // PrivateExpressionBodied on line 22 (1-based), col 33 (0-based)
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    21,
                    33),

                // PrivateBranchFastPath on line 28 (1-based), col 36 (0-based)
                new ExpectedDiagnosticModel(
                    ProofFilePath,
                    DiagnosticSeverity.Info,
                    27,
                    36),
            ];
        }
    }
}
