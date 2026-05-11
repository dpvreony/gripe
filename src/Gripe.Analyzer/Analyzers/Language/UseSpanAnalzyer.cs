// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to check for conditions where it may make sense to use Span{T}.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseSpanAnalzyer : DiagnosticAnalyzer
    {
        private const string Title = "Consider using Span<T> for in-place memory manipulation";
        private const string MessageFormat = "Consider using Span<T> for optimized memory operations";
        private const string Category = "Performance";
        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSpanAnalzyer"/> class.
        /// </summary>
        public UseSpanAnalzyer()
        {
            _rule = new DiagnosticDescriptor(
                        DiagnosticIdsHelper.UseSpan,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Info,
                        isEnabledByDefault: true,
                        description: "Recommends Span<T> usage for memory-efficient in-place operations.");
        }

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            if (AnalyzeArrayCopyOrCloneUsage(context, invocation)) return;
            if (AnalyzeSubstringUsage(context, invocation)) return;
            if (AnalyzeLoopingOverSegments(context, invocation)) return;
            if (AnalyzeUnsafeMemoryManipulation(context, invocation)) return;
            if (AnalyzeStackAllocatedBuffers(context, invocation)) return;
            if (AnalyzeStringParsingAndTokenization(context, invocation)) return;
            if (AnalyzeBinarySerializationDeserialization(context, invocation)) return;
            if (AnalyzeInterfacingWithNativeCode(context, invocation)) return;
            if (AnalyzeEfficientStringComparison(context, invocation)) return;
        }

        private bool AnalyzeArrayCopyOrCloneUsage(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                (memberAccess.Name.Identifier.Text == "Copy" || memberAccess.Name.Identifier.Text == "Clone"))
            {
                if (context.SemanticModel.GetSymbolInfo(memberAccess.Expression).Symbol is IArrayTypeSymbol)
                {
                    var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using Span<T> instead of copying arrays.");
                    context.ReportDiagnostic(diagnostic);
                    return true;
                }
            }
            return false;
        }

        private bool AnalyzeSubstringUsage(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "Substring" &&
                context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type.SpecialType == SpecialType.System_String)
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using ReadOnlySpan<char> instead of Substring for non-copying string slicing.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private bool AnalyzeLoopingOverSegments(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Detects loops iterating over array segments.
            if (context.Node.Parent is ForStatementSyntax forLoop &&
                forLoop.Declaration?.Type is ArrayTypeSyntax arrayType &&
                context.SemanticModel.GetTypeInfo(arrayType).Type is IArrayTypeSymbol)
            {
                var diagnostic = Diagnostic.Create(_rule, forLoop.GetLocation(), "Consider using Span<T> to iterate over array segments.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private bool AnalyzeUnsafeMemoryManipulation(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Checks for fixed statements on arrays, suggesting Span<T> instead.
            if (context.Node.Parent is FixedStatementSyntax fixedStatement &&
                fixedStatement.Declaration.Type is PointerTypeSyntax)
            {
                var diagnostic = Diagnostic.Create(_rule, fixedStatement.GetLocation(), "Consider using Span<T> instead of fixed statements for safer memory access.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private bool AnalyzeStackAllocatedBuffers(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Looks for small byte arrays that could be replaced with stackalloc Span<T>.
            if (invocation.Expression is ObjectCreationExpressionSyntax objCreation &&
                objCreation.Type is ArrayTypeSyntax arrayType &&
                arrayType.ElementType.ToString() == "byte" &&
                arrayType.RankSpecifiers.FirstOrDefault()?.Sizes.FirstOrDefault() is LiteralExpressionSyntax sizeLiteral &&
                sizeLiteral.Token.Value is int size && size <= 512) // threshold size
            {
                var diagnostic = Diagnostic.Create(_rule, objCreation.GetLocation(), "Consider using stackalloc Span<byte> for small buffers.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private  bool AnalyzeStringParsingAndTokenization(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Detects string.Split or custom parsing that could be optimized using ReadOnlySpan<char>.
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "Split" &&
                context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type.SpecialType == SpecialType.System_String)
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using ReadOnlySpan<char> for tokenization to avoid intermediate strings.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private  bool AnalyzeBinarySerializationDeserialization(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Detects calls to BitConverter methods on byte arrays that could be refactored with Span<byte>.
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                (memberAccess.Name.Identifier.Text.StartsWith("To") || memberAccess.Name.Identifier.Text.StartsWith("Get")) &&
                context.SemanticModel.GetSymbolInfo(memberAccess.Expression).Symbol?.ContainingType.Name == "BitConverter")
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using Span<byte> for efficient binary data manipulation.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private bool AnalyzeInterfacingWithNativeCode(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Looks for arrays passed to P/Invoke methods that could be Span<byte>.
            if (context.SemanticModel.GetSymbolInfo(invocation.Expression).Symbol is IMethodSymbol method &&
                method.GetAttributes().Any(attr => attr.AttributeClass?.Name == "DllImportAttribute") &&
                invocation.ArgumentList.Arguments.Any(arg => context.SemanticModel.GetTypeInfo(arg.Expression).Type is IArrayTypeSymbol arraySymbol &&
                                                             arraySymbol.ElementType.SpecialType == SpecialType.System_Byte))
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using Span<byte> for efficient P/Invoke calls.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }

        private bool AnalyzeEfficientStringComparison(SyntaxNodeAnalysisContext context, InvocationExpressionSyntax invocation)
        {
            // Looks for substring comparisons that could use ReadOnlySpan<char> to avoid intermediate strings.
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "Equals" &&
                memberAccess.Expression is InvocationExpressionSyntax innerInvocation &&
                innerInvocation.Expression is MemberAccessExpressionSyntax innerMember &&
                innerMember.Name.Identifier.Text == "Substring" &&
                context.SemanticModel.GetTypeInfo(innerMember.Expression).Type.SpecialType == SpecialType.System_String)
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(), "Consider using ReadOnlySpan<char> for efficient string comparisons.");
                context.ReportDiagnostic(diagnostic);
                return true;
            }
            return false;
        }
    }
}
