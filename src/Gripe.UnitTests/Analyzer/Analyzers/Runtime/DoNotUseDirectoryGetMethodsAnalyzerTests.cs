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
                    DiagnosticSeverity.Warning,
                    20,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    34,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    47,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    61,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    74,
                    16),
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseDirectoryGetMethodsAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    88,
                    16),
            ];
        }
    }
}
