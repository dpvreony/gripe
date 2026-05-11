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
    /// Analyzer to check for BitConverter usage that could be replaced with Span{byte}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForBitConverterAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<byte> for binary data manipulation";
        private const string MessageFormat = "Consider using Span<byte> for efficient binary data manipulation";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForBitConverterAnalyzer"/> class.
        /// </summary>
        public UseSpanForBitConverterAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForBitConverter,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true,
                        description: "Recommends Span<byte> usage for efficient binary data manipulation.");
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
                (memberAccess.Name.Identifier.Text.StartsWith("To") || memberAccess.Name.Identifier.Text.StartsWith("Get")))
            {
                var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
                if (methodSymbol?.ContainingType?.Name == "BitConverter" &&
                    methodSymbol.ContainingType.ContainingNamespace?.ToDisplayString() == "System")
                {
                    var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
