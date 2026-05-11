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
    /// Analyzer to check for small byte arrays that could be replaced with stackalloc Span{T}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForStackAllocBuffersAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using stackalloc Span<byte> for small buffers";
        private const string MessageFormat = "Consider using stackalloc Span<byte> for small buffers";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForStackAllocBuffersAnalyzer"/> class.
        /// </summary>
        public UseSpanForStackAllocBuffersAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForStackAllocBuffers,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends stackalloc Span<byte> usage for small buffers.");
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.ArrayCreationExpression);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var arrayCreation = (ArrayCreationExpressionSyntax)context.Node;

            if (arrayCreation.Type is ArrayTypeSyntax arrayType &&
                arrayType.ElementType.ToString() == "byte" &&
                arrayType.RankSpecifiers.FirstOrDefault()?.Sizes.FirstOrDefault() is LiteralExpressionSyntax sizeLiteral &&
                sizeLiteral.Token.Value is int size && size <= 512)
            {
                var diagnostic = Diagnostic.Create(_rule, arrayCreation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
