// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to check for Array.Copy or Clone usage that could be replaced with Span{T}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForArrayCopyAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<T> instead of copying arrays";
        private const string MessageFormat = "Consider using Span<T> instead of copying arrays";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForArrayCopyAnalyzer"/> class.
        /// </summary>
        public UseSpanForArrayCopyAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForArrayCopy,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends Span<T> usage instead of Array.Copy or Clone for memory-efficient operations.");
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                (memberAccess.Name.Identifier.Text == "Copy" || memberAccess.Name.Identifier.Text == "Clone"))
            {
                if (context.SemanticModel.GetSymbolInfo(memberAccess.Expression).Symbol is IArrayTypeSymbol)
                {
                    var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
