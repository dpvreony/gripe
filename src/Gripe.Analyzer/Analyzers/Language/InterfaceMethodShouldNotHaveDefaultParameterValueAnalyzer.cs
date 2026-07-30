// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to ensure interface methods do not define default parameter values.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Interface methods should not define default parameter values.";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Design;

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer"/> class.
        /// </summary>
        public InterfaceMethodShouldNotHaveDefaultParameterValueAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.InterfaceMethodShouldNotHaveDefaultParameterValue,
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: DiagnosticResultDescriptionFactory.InterfaceMethodShouldNotHaveDefaultParameterValue());
        }

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not MethodDeclarationSyntax methodDeclaration)
            {
                return;
            }

            if (methodDeclaration.Parent is not InterfaceDeclarationSyntax)
            {
                return;
            }

            foreach (var parameter in methodDeclaration.ParameterList.Parameters)
            {
                if (parameter.Default != null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(_rule, parameter.Default.GetLocation()));
                }
            }
        }
    }
}
