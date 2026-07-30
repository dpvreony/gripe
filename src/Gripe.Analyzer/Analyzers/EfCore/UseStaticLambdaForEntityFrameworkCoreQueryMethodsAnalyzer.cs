// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.EfCore
{
    /// <summary>
    /// Analyzer to suggest using static lambda in EF Core LINQ query methods.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Use static lambda for EF Core query method predicate arguments.";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Performance;

        private const string Description =
            "Using static lambdas in EF Core query methods prevents accidental capture of instance data and offers small performance benefits for the compiler.";

        private static readonly string[] TargetMethodNames =
        [
            "Include",
            "Select",
            "GroupBy",
            "ThenBy",
            "OrderBy",
            "OrderByDescending",
        ];

        private static readonly string[] AllowedContainingTypes =
        [
            "System.Linq.Queryable",
            "Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions",
        ];

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzer"/> class.
        /// </summary>
        public UseStaticLambdaForEntityFrameworkCoreQueryMethodsAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.UseStaticLambdaForEntityFrameworkCoreQueryMethods,
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;

            var memberExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;
            if (memberExpression == null)
            {
                return;
            }

            var methodName = memberExpression.Name.ToString();
            if (!Array.Exists(TargetMethodNames, m => string.Equals(m, methodName, StringComparison.Ordinal)))
            {
                return;
            }

            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpression);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
            {
                return;
            }

            var containingTypeName = methodSymbol.ContainingType.ToDisplayString();
            if (!Array.Exists(AllowedContainingTypes, t => string.Equals(t, containingTypeName, StringComparison.Ordinal)))
            {
                return;
            }

            foreach (var argument in invocationExpression.ArgumentList.Arguments)
            {
                if (argument.Expression is SimpleLambdaExpressionSyntax simpleLambda
                    && !simpleLambda.Modifiers.Any(SyntaxKind.StaticKeyword))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_rule, simpleLambda.GetLocation()));
                }
                else if (argument.Expression is ParenthesizedLambdaExpressionSyntax parenthesizedLambda
                    && !parenthesizedLambda.Modifiers.Any(SyntaxKind.StaticKeyword))
                {
                    context.ReportDiagnostic(Diagnostic.Create(_rule, parenthesizedLambda.GetLocation()));
                }
            }
        }
    }
}
