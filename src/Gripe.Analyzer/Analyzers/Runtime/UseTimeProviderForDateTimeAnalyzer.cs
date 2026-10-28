// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Analyzer for direct access to the system clock.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseTimeProviderForDateTimeAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Consider using TimeProvider for time-sensitive operations";

        private const string MessageFormat = Title;

        private const string Category = Gripe.Analyzer.CodeCracker.Extensions.SupportedCategories.Reliability;

        private const string Description =
            "Replacing direct static clock access with an injected TimeProvider makes time-dependent code easier to test.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticIdsHelper.UseTimeProviderForDateTime,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
        }

        private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;
            if (context.SemanticModel.GetSymbolInfo(memberAccess).Symbol is not IPropertySymbol property
                || !property.IsStatic
                || (property.Name != "Now" && property.Name != "UtcNow" && property.Name != "Today"))
            {
                return;
            }

            var containingType = property.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (containingType != "global::System.DateTime"
                && containingType != "global::System.DateTimeOffset")
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, memberAccess.GetLocation()));
        }
    }
}
