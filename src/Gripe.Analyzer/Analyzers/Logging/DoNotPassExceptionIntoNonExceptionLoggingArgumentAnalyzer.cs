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

namespace Gripe.Analyzer.Analyzers.Logging
{
    /// <summary>
    /// Analyzer for checking exceptions are passed into explicit logging exception parameters.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Do not pass System.Exception into a non-System.Exception logging argument.";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Design;

        private const string LoggingNamespace = "Microsoft.Extensions.Logging";

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer"/> class.
        /// </summary>
        public DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.DoNotPassExceptionIntoNonExceptionLoggingArgument,
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: DiagnosticResultDescriptionFactory.DoNotPassExceptionIntoNonExceptionLoggingArgument());
        }

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
        }

        private static bool IsExceptionType(
            ITypeSymbol typeSymbol,
            INamedTypeSymbol exceptionTypeSymbol)
        {
            var currentType = typeSymbol;
            while (currentType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(currentType, exceptionTypeSymbol))
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        private static bool IsLoggingMethod(IMethodSymbol targetMethod)
        {
            return targetMethod.Name.StartsWith("Log", StringComparison.Ordinal)
                   && targetMethod.ContainingNamespace.ToDisplayString().Equals(
                       LoggingNamespace,
                       StringComparison.Ordinal)
                   && (
                       targetMethod.ContainingType.Name.Equals("LoggerExtensions", StringComparison.Ordinal)
                       || targetMethod.ContainingType.Name.Equals("ILogger", StringComparison.Ordinal));
        }

        private static bool IsSystemExceptionParameter(
            IParameterSymbol parameterSymbol,
            INamedTypeSymbol exceptionTypeSymbol)
        {
            return parameterSymbol != null
                   && IsExceptionType(parameterSymbol.Type, exceptionTypeSymbol);
        }

        private static IMethodSymbol GetMethodSymbol(
            SyntaxNodeAnalysisContext context,
            InvocationExpressionSyntax invocationExpression)
        {
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpression, context.CancellationToken);
            if (symbolInfo.Symbol is IMethodSymbol methodSymbol)
            {
                return methodSymbol;
            }

            foreach (var candidateSymbol in symbolInfo.CandidateSymbols)
            {
                if (candidateSymbol is IMethodSymbol candidateMethodSymbol
                    && IsLoggingMethod(candidateMethodSymbol))
                {
                    return candidateMethodSymbol;
                }
            }

            return null;
        }

        private static IParameterSymbol GetParameterSymbol(
            ImmutableArray<IParameterSymbol> parameters,
            ArgumentSyntax argumentSyntax,
            ref int positionalParameterIndex)
        {
            if (argumentSyntax.NameColon != null)
            {
                var parameterName = argumentSyntax.NameColon.Name.Identifier.ValueText;
                foreach (var parameterSymbol in parameters)
                {
                    if (parameterSymbol.Name.Equals(parameterName, StringComparison.Ordinal))
                    {
                        return parameterSymbol;
                    }
                }

                return null;
            }

            if (positionalParameterIndex >= parameters.Length)
            {
                return null;
            }

            var parameter = parameters[positionalParameterIndex];
            if (!parameter.IsParams || positionalParameterIndex < parameters.Length - 1)
            {
                positionalParameterIndex++;
            }

            return parameter;
        }

        private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;
            var methodSymbol = GetMethodSymbol(context, invocationExpression);
            if (methodSymbol == null)
            {
                return;
            }

            if (!IsLoggingMethod(methodSymbol))
            {
                return;
            }

            var exceptionTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Exception");
            if (exceptionTypeSymbol == null)
            {
                return;
            }

            int positionalParameterIndex = 0;
            foreach (var argumentSyntax in invocationExpression.ArgumentList.Arguments)
            {
                var parameterSymbol = GetParameterSymbol(methodSymbol.Parameters, argumentSyntax, ref positionalParameterIndex);
                var typeInfo = context.SemanticModel.GetTypeInfo(argumentSyntax.Expression, context.CancellationToken);
                if (!IsExceptionType(typeInfo.Type, exceptionTypeSymbol)
                    || IsSystemExceptionParameter(parameterSymbol, exceptionTypeSymbol))
                {
                    continue;
                }

                context.ReportDiagnostic(Diagnostic.Create(_rule, argumentSyntax.GetLocation()));
            }
        }
    }
}
