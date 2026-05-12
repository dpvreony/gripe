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
    /// Analyzer to check if abstract classes without implementations should be interfaces.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Abstract classes without method implementations should probably be interfaces";

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer"/> class.
        /// </summary>
        public AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfaces,
                Title,
                Title,
                SupportedCategories.Design,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Title);
        }

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclarationExpression, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClassDeclarationExpression(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax classDeclarationSyntax))
            {
                return;
            }

            if (!classDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
            {
                return;
            }

            if (HasMethodImplementation(classDeclarationSyntax))
            {
                return;
            }

            var identifier = classDeclarationSyntax.Identifier;
            context.ReportDiagnostic(Diagnostic.Create(_rule, identifier.GetLocation()));
        }

        private static bool HasMethodImplementation(ClassDeclarationSyntax classDeclarationSyntax)
        {
            foreach (var member in classDeclarationSyntax.Members)
            {
                switch (member)
                {
                    case MethodDeclarationSyntax methodDeclarationSyntax:
                        if (!methodDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
                        {
                            return true;
                        }

                        break;
                    case ConstructorDeclarationSyntax _:
                    case DestructorDeclarationSyntax _:
                    case OperatorDeclarationSyntax _:
                    case ConversionOperatorDeclarationSyntax _:
                        return true;
                }
            }

            return false;
        }
    }
}
