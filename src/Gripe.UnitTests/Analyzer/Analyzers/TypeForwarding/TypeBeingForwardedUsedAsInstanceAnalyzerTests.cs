using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.TypeForwarding;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;

namespace Gripe.UnitTests.Analyzer.Analyzers.TypeForwarding
{
    /// <summary>
    /// Unit Tests for <see cref="TypeBeingForwardedUsedAsInstanceAnalyzer"/>.
    /// </summary>
    public sealed class TypeBeingForwardedUsedAsInstanceAnalyzerTests : AbstractAnalyzerTest<TypeBeingForwardedUsedAsInstanceAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.TypeBeingForwardedUsedAsInstance;
        }

        /// <inheritdoc />
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "TypeForwarding\\TypeBeingForwardedProof.cs",
                    Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
                    47,
                    27),
            ];
        }
    }
}
