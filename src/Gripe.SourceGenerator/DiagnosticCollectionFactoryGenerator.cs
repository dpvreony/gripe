// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gripe.SourceGenerator
{
    /// <summary>
    /// Source generator to integrate analyzers into the dotnet tool.
    /// </summary>
    [Generator]
    public sealed class DiagnosticCollectionFactoryGenerator : IIncrementalGenerator
    {
        /// <inheritdoc/>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var trigger = context.CompilationProvider.Combine(context.MetadataReferencesProvider.Collect());

            context.RegisterImplementationSourceOutput(trigger, static (spc, trigger) => Execute(spc, trigger.Left, trigger.Right));
        }

        private static void Execute(SourceProductionContext spc, Compilation compilation, ImmutableArray<MetadataReference> metadataReferences)
        {
            // the secondary equals check is because the test environment loads compilation references without a path.
            var analyzerRef = metadataReferences.FirstOrDefault(x =>
                x.Display != null
                && (
                    x.Display!.EndsWith("\\Gripe.Analyzer.dll", StringComparison.Ordinal)
                    || x.Display!.EndsWith("/Gripe.Analyzer.dll", StringComparison.Ordinal)
                    || x.Display!.Equals("Gripe.Analyzer", StringComparison.Ordinal)));

            if (analyzerRef == null)
            {
                spc.ReportDiagnostic(ErrorDiagnostic("GRSC0001", "Failed to find analyzer project reference"));
                return;
            }

            // Resolve the DiagnosticAnalyzer base type symbol.
            var diagnosticAnalyzerSymbol = compilation.GetTypeByMetadataName("Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer");
            if (diagnosticAnalyzerSymbol == null)
            {
                spc.ReportDiagnostic(ErrorDiagnostic("GRSC0002", "Failed to find DiagnosticAnalyzer underlying type symbol."));
                return;
            }

            // Find all public named types that inherit from DiagnosticAnalyzer.
            var found = FindPublicDiagnosticAnalyzers(compilation, diagnosticAnalyzerSymbol);

            if (found.Length == 0)
            {
                spc.ReportDiagnostic(ErrorDiagnostic("GRSC0003", "No public analyzers found to generate factory for."));
                return;
            }

            var namespaceDeclaration = GenerateNamespaceDeclarationSyntax(found);

            var cu = SyntaxFactory.CompilationUnit()
                .AddMembers(namespaceDeclaration)
                .NormalizeWhitespace();

            var sourceText = SyntaxFactory.SyntaxTree(
                    cu,
                    null,
                    encoding: Encoding.UTF8)
                .GetText();

            spc.AddSource("DiagnosticAnalyzerCollectionFactory.g.cs", sourceText);
        }

        private static NamespaceDeclarationSyntax GenerateNamespaceDeclarationSyntax(ImmutableArray<INamedTypeSymbol> found)
        {
            var classDeclaration = GenerateClassDeclarationSyntax(found);

            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("Gripe.DotNetTool"));
            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            return namespaceDeclaration;
        }

        private static ClassDeclarationSyntax GenerateClassDeclarationSyntax(ImmutableArray<INamedTypeSymbol> found)
        {
            var methodDeclaration = GenerateMethodDeclarationSyntax(found);

            var classDeclaration = SyntaxFactory.ClassDeclaration("DiagnosticAnalyzerCollectionFactory")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddMembers(methodDeclaration)
                .WithLeadingTrivia(GenerateSummaryComment("Factory class for getting the Diagnostic Analyzers."));
            return classDeclaration;
        }

        private static MethodDeclarationSyntax GenerateMethodDeclarationSyntax(ImmutableArray<INamedTypeSymbol> found)
        {
            var methodModifiers = SyntaxFactory.TokenList(
                SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                SyntaxFactory.Token(SyntaxKind.StaticKeyword));
            var methodReturn = SyntaxFactory.ParseTypeName("System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer>");
            var methodIdentifier = SyntaxFactory.Identifier("EnumerateDiagnosticAnalyzers");
            var methodBody = SyntaxFactory.Block();

            foreach (var f in found)
            {
                var fq = f.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var analyzerInstanceExpression = SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.ParseTypeName(fq))
                    .WithArgumentList(SyntaxFactory.ArgumentList());
                methodBody = methodBody.AddStatements(SyntaxFactory.YieldStatement(SyntaxKind.YieldReturnStatement, analyzerInstanceExpression));
            }

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                    methodReturn,
                    methodIdentifier)
                .WithModifiers(methodModifiers)
                .WithBody(methodBody)
                .WithLeadingTrivia(GenerateMethodSummaryComment("Gets an enumerable for the Diagnostic Analyzers.", null, "Enumerable representing the collection of Analyzers."));
            return methodDeclaration;
        }

        private static ImmutableArray<INamedTypeSymbol> FindPublicDiagnosticAnalyzers(Compilation compilation, INamedTypeSymbol diagnosticAnalyzerSymbol)
        {
            var results = new List<INamedTypeSymbol>();

            ProcessNamespace(compilation.GlobalNamespace, diagnosticAnalyzerSymbol, results);

            return results.ToImmutableArray();
        }

        private static void ProcessNamespace(INamespaceSymbol ns, INamedTypeSymbol diagnosticAnalyzerSymbol, IList<INamedTypeSymbol> results)
        {
            foreach (var member in ns.GetMembers())
            {
                if (member is INamespaceSymbol childNs)
                {
                    ProcessNamespace(childNs, diagnosticAnalyzerSymbol, results);
                }
                else if (member is INamedTypeSymbol namedType)
                {
                    ProcessNamedType(namedType, diagnosticAnalyzerSymbol, results);
                }
            }
        }

        private static void ProcessNamedType(INamedTypeSymbol namedType, INamedTypeSymbol diagnosticAnalyzerSymbol, IList<INamedTypeSymbol> results)
        {
            // Check declared accessibility is public.
            if (namedType is
                {
                    DeclaredAccessibility: Accessibility.Public,
                    IsAbstract: false,
                    IsGenericType: false
                }

                && InheritsFrom(namedType, diagnosticAnalyzerSymbol))
            {
                results.Add(namedType);
            }

            // Recurse into nested types.
            foreach (var nested in namedType.GetTypeMembers())
            {
                ProcessNamedType(nested, diagnosticAnalyzerSymbol, results);
            }
        }

        private static bool InheritsFrom(INamedTypeSymbol symbol, INamedTypeSymbol baseType)
        {
            var current = symbol.BaseType;
            while (current != null)
            {
                if (SymbolEqualityComparer.Default.Equals(current, baseType))
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        private static Diagnostic ErrorDiagnostic(
            string id,
            string message)
        {
            return GetDiagnostic(
                id,
                message,
                DiagnosticSeverity.Error,
                0);
        }

        private static Diagnostic GetDiagnostic(
            string id,
            string message,
            DiagnosticSeverity severity,
            int warningLevel)
        {
            return Diagnostic.Create(
                id,
                "Vetuviem Generation",
                message,
                severity,
                severity,
                true,
                warningLevel,
                "Model Generation");
        }

        private static SyntaxTriviaList GenerateSummaryComment(string summaryText)
        {
            var template = "/// <summary>\n" +
                           $"/// {summaryText}\n" +
                           "/// </summary>\n";

            return SyntaxFactory.ParseLeadingTrivia(template);
        }

        private static SyntaxTriviaList GenerateMethodSummaryComment(string summaryText, IEnumerable<(string ParamName, string ParamText)>? parameters, string returnValueText)
        {
            var sb = new StringBuilder("/// <summary>")
                .AppendLine()
                .Append("/// ").AppendLine(summaryText)
                .AppendLine("/// </summary>");

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    sb.Append("/// <param name=\"").Append(parameter.ParamName).Append("\">").Append(parameter.ParamText).AppendLine("</param>");
                }
            }

            sb.Append("/// <returns>").Append(returnValueText).AppendLine("</returns>");
            return SyntaxFactory.ParseLeadingTrivia(sb.ToString());
        }
    }
}