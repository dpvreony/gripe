// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to check for byte arrays passed to P/Invoke that could use Span{byte}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForPInvokeAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<byte> for P/Invoke calls";
        private const string MessageFormat = "Consider using Span<byte> for efficient P/Invoke calls";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForPInvokeAnalyzer"/> class.
        /// </summary>
        public UseSpanForPInvokeAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForPInvoke,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true,
                        description: "Recommends Span<byte> usage for efficient P/Invoke calls.");
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

            if (context.SemanticModel.GetSymbolInfo(invocation.Expression).Symbol is IMethodSymbol method &&
                method.GetAttributes().Any(attr => attr.AttributeClass?.Name == "DllImportAttribute") &&
                invocation.ArgumentList.Arguments.Any(arg => context.SemanticModel.GetTypeInfo(arg.Expression).Type is IArrayTypeSymbol arraySymbol &&
                                                             arraySymbol.ElementType.SpecialType == SpecialType.System_Byte))
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
