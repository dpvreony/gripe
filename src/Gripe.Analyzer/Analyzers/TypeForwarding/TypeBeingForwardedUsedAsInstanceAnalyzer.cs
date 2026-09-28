using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.TypeForwarding
{
    /// <summary>
    /// Analyzer that warns when a type that is being forwarded via an assembly attribute
    /// is instantiated in the same source file that declares the forwarding attribute.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class TypeBeingForwardedUsedAsInstanceAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticIdsHelper.TypeBeingForwardedUsedAsInstance,
            "Type being forwarded used as instance",
            "Type being forwarded '{0}' is instantiated",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initializes the analyzer.
        /// </summary>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                var assembly = compilationStartContext.Compilation.Assembly;
                var forwarded = GetForwardedTypeFullNames(assembly);
                if (forwarded.Count == 0)
                {
                    return;
                }

                compilationStartContext.RegisterSyntaxNodeAction(ctx => AnalyzeObjectCreation(ctx, forwarded), SyntaxKind.ObjectCreationExpression);
            });
        }

        private static HashSet<string> GetForwardedTypeFullNames(IAssemblySymbol assembly)
        {
            return GetForwardedTypeFullNames(assembly, null);
        }

        private static HashSet<string> GetForwardedTypeFullNames(IAssemblySymbol assembly, SyntaxTree? syntaxTree)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (var attr in assembly.GetAttributes())
            {
                if (syntaxTree != null)
                {
                    var appRef = attr.ApplicationSyntaxReference;
                    if (appRef == null || appRef.SyntaxTree != syntaxTree)
                    {
                        continue;
                    }
                }
                if (attr.AttributeClass == null)
                {
                    continue;
                }

                if (attr.AttributeClass.ToDisplayString() == "System.Runtime.CompilerServices.TypeForwardedToAttribute")
                {
                    if (attr.ConstructorArguments.Length > 0)
                    {
                        var tc = attr.ConstructorArguments[0];
                        if (tc.Kind == TypedConstantKind.Type && tc.Value is INamedTypeSymbol t)
                        {
                            set.Add(t.GetFullName());
                        }
                    }
                }
            }

            return set;
        }

        private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context, HashSet<string> forwarded)
        {
            var objCreation = (ObjectCreationExpressionSyntax)context.Node;
            // Re-evaluate forwarded types for the current syntax tree so only attributes
            // declared in the same file are considered.
            forwarded = GetForwardedTypeFullNames(context.SemanticModel.Compilation.Assembly, context.Node.SyntaxTree);

            var ctor = context.SemanticModel.GetSymbolInfo(objCreation).Symbol as IMethodSymbol;
            if (ctor == null)
            {
                return;
            }

            var containingType = ctor.ContainingType;
            if (containingType == null)
            {
                return;
            }

            var full = containingType.GetFullName();
            if (forwarded.Contains(full))
            {
                var diag = Diagnostic.Create(Rule, objCreation.GetLocation(), full);
                context.ReportDiagnostic(diag);
            }
        }
    }
}
