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
    /// Analyzer to check for loops iterating over array segments that could use Span{T}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForArraySegmentLoopsAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<T> to iterate over array segments";
        private const string MessageFormat = "Consider using Span<T> to iterate over array segments";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForArraySegmentLoopsAnalyzer"/> class.
        /// </summary>
        public UseSpanForArraySegmentLoopsAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForArraySegmentLoops,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends Span<T> usage for iterating over array segments.");
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ForStatement);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var forLoop = (ForStatementSyntax)context.Node;

            if (forLoop.Declaration?.Type is ArrayTypeSyntax arrayType &&
                context.SemanticModel.GetTypeInfo(arrayType).Type is IArrayTypeSymbol)
            {
                var diagnostic = Diagnostic.Create(_rule, forLoop.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
