// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to suggest using <c>ValueTask&lt;T&gt;</c> instead of <c>Task&lt;T&gt;</c> when synchronous
    /// completion is evident and allocation avoidance would be beneficial.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ConsiderValueTaskInsteadOfTaskAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Consider ValueTask<T> instead of Task<T>";

        private const string MessageFormat =
            "Consider returning ValueTask<T> instead of Task<T> to avoid allocation on synchronous completion paths";

        private const string Description =
            "When a method frequently completes synchronously (e.g. via Task.FromResult or a fast-path branch), " +
            "returning ValueTask<T> avoids a heap allocation. This is a performance suggestion only; " +
            "ValueTask<T> has stricter consumption rules and is not universally better than Task<T>.";

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/> class.
        /// </summary>
        public ConsiderValueTaskInsteadOfTaskAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.ConsiderValueTaskInsteadOfTask,
                Title,
                MessageFormat,
                SupportedCategories.Performance,
                DiagnosticSeverity.Info,
                isEnabledByDefault: true,
                description: Description);
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            // Skip methods with risky modifiers: abstract, virtual, override
            if (HasRiskyModifiers(methodDeclaration))
            {
                return;
            }

            // Skip explicit interface implementations
            if (methodDeclaration.ExplicitInterfaceSpecifier != null)
            {
                return;
            }

            // Skip async methods; detecting synchronous early-return paths in async methods
            // requires more complex analysis and risks false positives.
            if (methodDeclaration.Modifiers.Any(SyntaxKind.AsyncKeyword))
            {
                return;
            }

            var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
            if (methodSymbol == null)
            {
                return;
            }

            // Only report for private or internal methods where changing the signature is low-risk
            if (!IsLowRiskAccessibility(methodSymbol))
            {
                return;
            }

            // Check return type is Task<T> (the generic form with exactly one type argument)
            if (!IsGenericTaskReturnType(methodSymbol, context.SemanticModel.Compilation))
            {
                return;
            }

            // Analyse the body for synchronous completion patterns
            if (!HasSynchronousCompletionPattern(methodDeclaration, context.SemanticModel))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(_rule, methodDeclaration.Identifier.GetLocation()));
        }

        private static bool HasRiskyModifiers(MethodDeclarationSyntax methodDeclaration)
        {
            var modifiers = methodDeclaration.Modifiers;
            return modifiers.Any(SyntaxKind.AbstractKeyword)
                   || modifiers.Any(SyntaxKind.VirtualKeyword)
                   || modifiers.Any(SyntaxKind.OverrideKeyword);
        }

        private static bool IsLowRiskAccessibility(IMethodSymbol methodSymbol)
        {
            return methodSymbol.DeclaredAccessibility == Accessibility.Private
                   || methodSymbol.DeclaredAccessibility == Accessibility.Internal;
        }

        private static bool IsGenericTaskReturnType(IMethodSymbol methodSymbol, Compilation compilation)
        {
            if (!(methodSymbol.ReturnType is INamedTypeSymbol returnType))
            {
                return false;
            }

            if (!returnType.IsGenericType || returnType.TypeArguments.Length != 1)
            {
                return false;
            }

            var taskGenericType = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");
            return taskGenericType != null
                   && SymbolEqualityComparer.Default.Equals(returnType.OriginalDefinition, taskGenericType);
        }

        private static bool HasSynchronousCompletionPattern(
            MethodDeclarationSyntax methodDeclaration,
            SemanticModel semanticModel)
        {
            // Expression body: M() => Task.FromResult(expr);
            if (methodDeclaration.ExpressionBody != null)
            {
                return IsTaskFromResult(methodDeclaration.ExpressionBody.Expression, semanticModel);
            }

            // Block body: analyse return statements
            if (methodDeclaration.Body != null)
            {
                return AnalyzeBlockBody(methodDeclaration.Body, semanticModel);
            }

            return false;
        }

        private static bool AnalyzeBlockBody(BlockSyntax body, SemanticModel semanticModel)
        {
            var returnStatements = body.DescendantNodes()
                .OfType<ReturnStatementSyntax>()
                .ToList();

            if (returnStatements.Count == 0)
            {
                return false;
            }

            var hasTaskFromResult = false;

            foreach (var returnStatement in returnStatements)
            {
                if (returnStatement.Expression == null)
                {
                    return false;
                }

                if (IsTaskFromResult(returnStatement.Expression, semanticModel))
                {
                    hasTaskFromResult = true;
                }
                else if (IsMethodInvocationResult(returnStatement.Expression, semanticModel))
                {
                    // The return is a method call (likely an async helper); acceptable for a fast-path pattern
                }
                else
                {
                    // The return is a field, property, local variable, or unknown expression.
                    // Returning a stored Task<T> instance may be intentional (e.g. a shared/cached task),
                    // so skip conservatively.
                    return false;
                }
            }

            // Report only when at least one branch returns Task.FromResult
            return hasTaskFromResult;
        }

        private static bool IsTaskFromResult(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            if (!(expression is InvocationExpressionSyntax invocation))
            {
                return false;
            }

            var symbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (symbol == null)
            {
                return false;
            }

            return symbol.Name == "FromResult"
                   && symbol.ContainingType?.Name == "Task"
                   && symbol.ContainingType.ContainingNamespace?.ToDisplayString() == "System.Threading.Tasks";
        }

        private static bool IsMethodInvocationResult(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            if (!(expression is InvocationExpressionSyntax))
            {
                return false;
            }

            var symbol = semanticModel.GetSymbolInfo(expression).Symbol;
            return symbol is IMethodSymbol;
        }
    }
}
