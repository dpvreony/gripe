using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.TypeForwarding;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;

namespace Gripe.UnitTests.Analyzer.Analyzers.TypeForwarding
{
    /// <summary>
    /// Unit Tests for <see cref="TypeBeingForwardedUsedAsParameterAnalyzer"/>.
    /// </summary>
    public sealed class TypeBeingForwardedUsedAsParameterAnalyzerTests : AbstractAnalyzerTest<TypeBeingForwardedUsedAsParameterAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.TypeBeingForwardedUsedAsParameter;
        }

        /// <inheritdoc />
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "TypeForwarding\\TypeBeingForwardedProof.cs",
                    Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
                    33,
                    37),
            ];
        }
    }
}
