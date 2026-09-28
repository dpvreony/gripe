// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Linq;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gripe.Analyzer.Analyzers.Language
{
    /// <summary>
    /// Analyzer to identify classes that are good candidates for conversion to structs.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ClassCanBeStructAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "Class can be converted to struct";

        private const string MessageFormat = "Class '{0}' is small, immutable, and suitable to be a struct";

        private const string Description = "Prefer structs for small immutable value-like types to reduce heap allocations.";

        private readonly DiagnosticDescriptor _rule;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCanBeStructAnalyzer"/> class.
        /// </summary>
        public ClassCanBeStructAnalyzer()
        {
            _rule = new DiagnosticDescriptor(
                DiagnosticIdsHelper.ClassCanBeStruct,
                Title,
                MessageFormat,
                SupportedCategories.Performance,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);
        }

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSymbolAction(AnalyzeType, SymbolKind.NamedType);
        }

        private void AnalyzeType(SymbolAnalysisContext context)
        {
            var typeSymbol = (INamedTypeSymbol)context.Symbol;

            if (typeSymbol.TypeKind != TypeKind.Class || typeSymbol.IsAbstract || !typeSymbol.IsSealed)
            {
                return;
            }

            if (typeSymbol.BaseType?.SpecialType != SpecialType.System_Object)
            {
                return;
            }

            if (typeSymbol.GetMembers().OfType<IMethodSymbol>().Any(method => method.MethodKind == MethodKind.Destructor || method.IsVirtual || method.IsAbstract || method.IsOverride))
            {
                return;
            }

            var instanceFields = typeSymbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(field => !field.IsStatic)
                .ToArray();

            if (instanceFields.Length is < 1 or > 4)
            {
                return;
            }

            if (instanceFields.Any(field => !field.IsReadOnly || field.Type.IsReferenceType))
            {
                return;
            }

            if (typeSymbol.GetMembers().OfType<IPropertySymbol>().Any(property => !property.IsStatic && property.SetMethod != null))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(_rule, typeSymbol.Locations[0], typeSymbol.Name));
        }
    }
}
