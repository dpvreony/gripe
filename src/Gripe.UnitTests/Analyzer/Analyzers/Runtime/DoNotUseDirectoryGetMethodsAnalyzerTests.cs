using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Runtime;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Unit test for <see cref="DoNotUseDirectoryGetMethodsAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseDirectoryGetMethodsAnalyzerTests : AbstractAnalyzerTest<DoNotUseDirectoryGetMethodsAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseDirectoryGetMethods;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    20,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    34,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    47,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    61,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    74,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Error,
                    88,
                    16),
            ];
        }
    }
}
