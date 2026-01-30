using Gripe.Analyzer;
using Gripe.UnitTests.Analyzer.Analyzers.EfCore;
using Microsoft.CodeAnalysis;
using Gripe.Analyzer.Analyzers.Winforms;

namespace Gripe.UnitTests.Analyzer.Analyzers.Winforms
{
    /// <summary>
    /// Unit test for <see cref="Gripe.Analyzer.Analyzers.Winforms.MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzer"/>.
    /// </summary>
    public sealed class MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzerTests : AbstractAnalyzerTest<MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzer>
    {
        /// <inheritdoc/>
        protected override string GetExpectedDiagnosticId()
        {
            return DiagnosticIdsHelper.WinformsMessageBoxShowShouldBeCalledFromUiControl;
        }

        /// <inheritdoc/>
        protected override ExpectedDiagnosticModel[] GetExpectedDiagnosticLines()
        {
            return
            [
                new ExpectedDiagnosticModel(
                    "Winforms\\SimpleForm.cs",
                    DiagnosticSeverity.Error,
                    31,
                    12)
            ];
        }
    }
}
