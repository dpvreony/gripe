// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.EfCore
{
    /// <summary>
    /// Analyzer for checking a method that queries an Entity Framework Core DbContext does not return an entity, a projection should be used instead.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class MethodShouldNotReturnEntityFrameworkCoreEntityAnalyzer : DiagnosticAnalyzer
    {
        private const string Title = "Method should return a projection instead of an Entity Framework Core entity.";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Performance;

        private const string Description = "Returning an entity loads unnecessary data and exposes the persistence model. Use a projection (e.g. Select with specific fields) into a DTO instead.";

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodShouldNotReturnEntityFrameworkCoreEntityAnalyzer"/> class.
        /// </summary>
        public MethodShouldNotReturnEntityFrameworkCoreEntityAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.MethodShouldNotReturnEntityFrameworkCoreEntity,
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
            context.RegisterCompilationStartAction(OnCompilationStart);
        }

        private static bool IsDbSet(INamedTypeSymbol type)
        {
            return type.IsGenericType
                && type.TypeArguments.Length == 1
                && type.OriginalDefinition.Name == "DbSet"
                && type.OriginalDefinition.ContainingNamespace.ToDisplayString() == "Microsoft.EntityFrameworkCore";
        }

        private static void CollectEntities(INamespaceSymbol ns, INamedTypeSymbol dbContext, HashSet<ITypeSymbol> entities)
        {
            foreach (var member in ns.GetMembers())
            {
                if (member is INamespaceSymbol childNamespace)
                {
                    CollectEntities(childNamespace, dbContext, entities);
                }
                else if (member is INamedTypeSymbol type)
                {
                    CollectEntities(type, dbContext, entities);
                }
            }
        }

        private static void CollectEntities(INamedTypeSymbol type, INamedTypeSymbol dbContext, HashSet<ITypeSymbol> entities)
        {
            foreach (var nested in type.GetTypeMembers())
            {
                CollectEntities(nested, dbContext, entities);
            }

            if (!InheritsFrom(type, dbContext))
            {
                return;
            }

            for (var current = type; current != null; current = current.BaseType)
            {
                foreach (var member in current.GetMembers())
                {
                    if (member is IPropertySymbol { Type: INamedTypeSymbol propertyType } && IsDbSet(propertyType))
                    {
                        entities.Add(propertyType.TypeArguments[0].WithNullableAnnotation(NullableAnnotation.None));
                    }
                }
            }
        }

        private static bool ReferencesEntityFrameworkCore(IAssemblySymbol assembly)
        {
            foreach (var module in assembly.Modules)
            {
                foreach (var referenced in module.ReferencedAssemblies)
                {
                    if (referenced.Name == "Microsoft.EntityFrameworkCore")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool InheritsFrom(INamedTypeSymbol type, INamedTypeSymbol baseType)
        {
            for (var current = type.BaseType; current != null; current = current.BaseType)
            {
                if (SymbolEqualityComparer.Default.Equals(current.OriginalDefinition, baseType))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool UsesDbContext(MethodDeclarationSyntax method, SemanticModel semanticModel, INamedTypeSymbol dbContext, System.Threading.CancellationToken cancellationToken)
        {
            SyntaxNode? body = method.Body;
            body ??= method.ExpressionBody;
            if (body == null)
            {
                return false;
            }

            foreach (var node in body.DescendantNodes())
            {
                if (node is not ExpressionSyntax expression)
                {
                    continue;
                }

                if (semanticModel.GetTypeInfo(expression, cancellationToken).Type is not INamedTypeSymbol type)
                {
                    continue;
                }

                if (IsDbSet(type)
                    || SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, dbContext)
                    || InheritsFrom(type, dbContext))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ReturnsEntity(ITypeSymbol type, HashSet<ITypeSymbol> entities, int depth)
        {
            if (depth > 4)
            {
                return false;
            }

            type = type.WithNullableAnnotation(NullableAnnotation.None);

            if (entities.Contains(type))
            {
                return true;
            }

            if (type is IArrayTypeSymbol arrayType)
            {
                return ReturnsEntity(arrayType.ElementType, entities, depth + 1);
            }

            if (type is INamedTypeSymbol { IsGenericType: true } namedType)
            {
                foreach (var typeArgument in namedType.TypeArguments)
                {
                    if (ReturnsEntity(typeArgument, entities, depth + 1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnCompilationStart(CompilationStartAnalysisContext context)
        {
            var dbContext = context.Compilation.GetTypeByMetadataName("Microsoft.EntityFrameworkCore.DbContext");
            if (dbContext == null)
            {
                return;
            }

            var entities = new Lazy<HashSet<ITypeSymbol>>(
                () =>
                {
                    var set = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
                    CollectEntities(context.Compilation.Assembly.GlobalNamespace, dbContext, set);
                    foreach (var reference in context.Compilation.References)
                    {
                        if (context.Compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly
                            && ReferencesEntityFrameworkCore(assembly))
                        {
                            CollectEntities(assembly.GlobalNamespace, dbContext, set);
                        }
                    }

                    return set;
                },
                true);

            context.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeMethod(nodeContext, entities.Value, dbContext),
                SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethod(SyntaxNodeAnalysisContext context, HashSet<ITypeSymbol> entities, INamedTypeSymbol dbContext)
        {
            if (entities.Count == 0)
            {
                return;
            }

            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            if (context.SemanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken) is not IMethodSymbol methodSymbol)
            {
                return;
            }

            if (ReturnsEntity(methodSymbol.ReturnType, entities, 0)
                && UsesDbContext(methodDeclaration, context.SemanticModel, dbContext, context.CancellationToken))
            {
                context.ReportDiagnostic(Diagnostic.Create(_rule, methodDeclaration.ReturnType.GetLocation()));
            }
        }
    }
}
