using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Runtime;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;

namespace Gripe.UnitTests.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Unit test for <see cref="DoNotUseJsonConstructorAnalyzer"/>.
    /// </summary>
    public sealed class DoNotUseJsonConstructorAnalyzerTests : AbstractAnalyzerTest<DoNotUseJsonConstructorAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.DoNotUseJsonConstructor;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "Runtime\\DoNotUseJsonConstructorAnalyzerProof.cs",
                    DiagnosticSeverity.Warning,
                    38,
                    16),
            ];
        }
    }
}
