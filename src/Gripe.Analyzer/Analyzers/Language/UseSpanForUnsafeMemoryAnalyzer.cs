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
    /// Analyzer to check for fixed statements that could be replaced with Span{T}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForUnsafeMemoryAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<T> instead of fixed statements";
        private const string MessageFormat = "Consider using Span<T> instead of fixed statements for safer memory access";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForUnsafeMemoryAnalyzer"/> class.
        /// </summary>
        public UseSpanForUnsafeMemoryAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForUnsafeMemory,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends Span<T> usage instead of fixed statements for safer memory access.");
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.FixedStatement);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var fixedStatement = (FixedStatementSyntax)context.Node;

            if (fixedStatement.Declaration.Type is PointerTypeSyntax)
            {
                var diagnostic = Diagnostic.Create(_rule, fixedStatement.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
