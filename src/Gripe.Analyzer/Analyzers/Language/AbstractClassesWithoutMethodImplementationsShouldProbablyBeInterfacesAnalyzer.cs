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

            if (HasMemberImplementation(classDeclarationSyntax))
            {
                return;
            }

            var identifier = classDeclarationSyntax.Identifier;
            context.ReportDiagnostic(Diagnostic.Create(_rule, identifier.GetLocation()));
        }

        private static bool HasMemberImplementation(ClassDeclarationSyntax classDeclarationSyntax)
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
                    case FieldDeclarationSyntax _:
                    case EventFieldDeclarationSyntax _:
                        return true;
                    case PropertyDeclarationSyntax propertyDeclarationSyntax:
                        if (!propertyDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
                        {
                            return true;
                        }

                        if (propertyDeclarationSyntax.ExpressionBody != null
                            || HasAccessorBodyImplementation(propertyDeclarationSyntax.AccessorList))
                        {
                            return true;
                        }

                        break;
                    case EventDeclarationSyntax eventDeclarationSyntax:
                        if (!eventDeclarationSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword))
                        {
                            return true;
                        }

                        if (HasAccessorBodyImplementation(eventDeclarationSyntax.AccessorList))
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        private static bool HasAccessorBodyImplementation(AccessorListSyntax accessorListSyntax)
        {
            if (accessorListSyntax?.Accessors == null)
            {
                return false;
            }

            foreach (var accessor in accessorListSyntax.Accessors)
            {
                if (accessor.Body != null || accessor.ExpressionBody != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
