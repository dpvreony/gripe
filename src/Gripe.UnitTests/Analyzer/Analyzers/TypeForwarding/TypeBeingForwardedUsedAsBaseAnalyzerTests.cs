using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.TypeForwarding;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;

namespace Gripe.UnitTests.Analyzer.Analyzers.TypeForwarding
{
    /// <summary>
    /// Unit Tests for <see cref="TypeBeingForwardedUsedAsBaseAnalyzer"/>.
    /// </summary>
    public sealed class TypeBeingForwardedUsedAsBaseAnalyzerTests : AbstractAnalyzerTest<TypeBeingForwardedUsedAsBaseAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.TypeBeingForwardedUsedAsBase;
        }

        /// <inheritdoc />
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "TypeForwarding\\TypeBeingForwardedProof.cs",
                    Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
                    19,
                    31),
            ];
        }
    }
}
