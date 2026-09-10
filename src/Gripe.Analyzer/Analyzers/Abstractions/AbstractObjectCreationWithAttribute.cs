using System;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Abstractions
{
    /// <summary>
    /// Base class for an analyzer that needs to warn about use of constructor that has a specific attribute.
    /// </summary>
    public abstract class AbstractObjectCreationWithAttribute : DiagnosticAnalyzer
    {
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObjectCreationUsingTypeAnalyzer"/> class.
        /// </summary>
        /// <param name="diagnosticId">The Diagnostic Id.</param>
        /// <param name="title">The title of the analyzer.</param>
        /// <param name="message">The message to display detailing the issue with the analyzer.</param>
        /// <param name="category">The category the analyzer belongs to.</param>
        /// <param name="description">The description of the analyzer.</param>
        /// <param name="diagnosticSeverity">The severity associated with breaches of the analyzer.</param>
        protected AbstractObjectCreationWithAttribute(
            [NotNull] string diagnosticId,
            [NotNull] string title,
            [NotNull] string message,
            [NotNull] string category,
            [NotNull] string description,
            DiagnosticSeverity diagnosticSeverity)
        {
            _rule = new DiagnosticDescriptor(
                diagnosticId,
                title,
                message,
                category,
                diagnosticSeverity,
                isEnabledByDefault: true,
                description: description);
        }

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <summary>
        /// Gets the fully qualified type name of the attribute to check for.
        /// </summary>
        [NotNull]
        protected abstract string AttributeFullTypeName { get; }

        /// <inheritdoc />
        public sealed override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreationExpression, SyntaxKind.ObjectCreationExpression);
        }

        private void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            var constructorSymbol = context.SemanticModel.GetSymbolInfo(objectCreationExpression).Symbol;

            if (constructorSymbol == null)
            {
                return;
            }

            var hasAttribute = constructorSymbol.GetAttributes().Any(attr => attr.AttributeClass?.ToDisplayString().Equals(AttributeFullTypeName, StringComparison.Ordinal) == true);

            if (!hasAttribute)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(_rule, objectCreationExpression.GetLocation()));
        }
    }
}
