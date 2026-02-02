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
                    30,
                    12)
            ];
        }
    }
}
