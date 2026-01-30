using Gripe.Analyzer.Analyzers.Abstractions;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Winforms
{
    /// <summary>
    /// Analyzer to ensure that ShowDialog is called from a UI element member.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzer : AbstractInvocationWithAdditionalCheckAnalyzer
    {
        internal const string Title = "MessageBox.Show should be called from a member of a UI control.";
        private const string MessageFormat = Title;
        private const string Category = SupportedCategories.Performance;
        private const string Description = "MessageBox.Show should be called from a non-static member of a UI control. This is to prevent issues where the MessageBox can't establish the Window handle and thus the message box is not modal.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzer"/> class.
        /// </summary>
        public MessageBoxShowShouldBeCalledFromUiElementMemberAnalyzer()
            : base(
                DiagnosticIdsHelper.WinformsMessageBoxShowShouldBeCalledFromUiControl,
                Title,
                MessageFormat,
                Category,
                Description,
                Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)
        {
        }

        /// <inheritdoc/>
        protected override string MethodName => "Show";

        /// <inheritdoc/>
        protected override string[] ContainingTypes => [
            "System.Windows.Forms.MessageBox",
        ];

        /// <inheritdoc/>
        protected override bool GetIfShouldReport(SemanticModel semanticModel, MemberAccessExpressionSyntax memberExpression)
        {
            // check if the calling method is static or not, and if not, if it belongs to a UI element.
            var methodDeclaration = memberExpression.FirstAncestorOrSelf<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>();
            if (methodDeclaration == null)
            {
                return true;
            }

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
            if (methodSymbol == null)
            {
                return true;
            }

            if (methodSymbol.IsStatic)
            {
                return true;
            }

            var containingType = methodSymbol.ContainingType;
            var baseType = containingType.BaseType;
            while (baseType != null)
            {
                var baseTypeName = baseType.GetFullName();

                if (baseTypeName.Equals("global::System.Windows.Forms.Control"))
                {
                    return false;
                }

                baseType = baseType.BaseType;
            }

            return true;
        }
    }
}
