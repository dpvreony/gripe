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
    /// Analyzer to check for substring comparisons that could use ReadOnlySpan{char}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanForStringComparisonAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using ReadOnlySpan<char> for string comparisons";
        private const string MessageFormat = "Consider using ReadOnlySpan<char> for efficient string comparisons";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanForStringComparisonAnalyzer"/> class.
        /// </summary>
        public UseSpanForStringComparisonAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpanForStringComparison,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends ReadOnlySpan<char> usage for efficient string comparisons.");
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
                memberAccess.Name.Identifier.Text == "Equals" &&
                memberAccess.Expression is InvocationExpressionSyntax innerInvocation &&
                innerInvocation.Expression is MemberAccessExpressionSyntax innerMember &&
                innerMember.Name.Identifier.Text == "Substring" &&
                context.SemanticModel.GetTypeInfo(innerMember.Expression).Type?.SpecialType == SpecialType.System_String)
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
