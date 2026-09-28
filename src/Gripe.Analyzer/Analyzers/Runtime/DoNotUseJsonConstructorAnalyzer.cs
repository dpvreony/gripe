// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Gripe.Analyzer.Analyzers.Abstractions;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gripe.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Analyzer to detect object creations that use constructors marked with [JsonConstructor].
    /// </summary>
    [Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotUseJsonConstructorAnalyzer : AbstractObjectCreationWithAttribute
    {
        private const string Title = "Do not create objects using constructors marked with [JsonConstructor]";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Usage;

        private const string Description = "Constructors marked with [JsonConstructor] are intended for deserialization and should not be invoked directly.";

        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotUseJsonConstructorAnalyzer"/> class.
        /// </summary>
        public DoNotUseJsonConstructorAnalyzer()
            : base(
                DiagnosticIdsHelper.DoNotUseJsonConstructor,
                Title,
                MessageFormat,
                Category,
                Description,
                DiagnosticSeverity.Warning)
        {
        }

        /// <inheritdoc />
        protected override string AttributeFullTypeName => "System.Text.Json.Serialization.JsonConstructorAttribute";

        /// <inheritdoc />
        protected override bool ShouldWarn(ObjectCreationExpressionSyntax objectCreationExpression, ISymbol constructorSymbol, Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext context)
        {
            if (constructorSymbol == null)
            {
                return true;
            }

            var containingType = constructorSymbol.ContainingType as INamedTypeSymbol;
            if (containingType == null)
            {
                return true;
            }

            var compilation = context.SemanticModel.Compilation;
            var currentAssembly = compilation.Assembly;

            int accessibleConstructors = 0;

            foreach (var ctor in containingType.Constructors)
            {
                switch (ctor.DeclaredAccessibility)
                {
                    // Count public constructors
                    case Accessibility.Public:
                    {
                        accessibleConstructors++;
                        if (accessibleConstructors > 1)
                        {
                            return true;
                        }
                        continue;
                    }
                    // Count internal / protected internal as accessible if the declaring assembly is the same as the current one
                    case Accessibility.Internal or Accessibility.ProtectedOrInternal:
                    {
                        var declaringAssembly = ctor.ContainingAssembly;
                        if (SymbolEqualityComparer.Default.Equals(declaringAssembly, currentAssembly))
                        {
                            accessibleConstructors++;
                            if (accessibleConstructors > 1)
                            {
                                return true;
                            }
                            continue;
                        }

                        // Check InternalsVisibleTo on the declaring assembly
                        foreach (var attr in declaringAssembly.GetAttributes())
                        {
                            if (attr.AttributeClass?.ToDisplayString() != "System.Runtime.CompilerServices.InternalsVisibleToAttribute")
                            {
                                continue;
                            }

                            if (attr.ConstructorArguments.Length == 0)
                            {
                                continue;
                            }

                            var ivt = attr.ConstructorArguments[0].Value as string;
                            if (ivt == null)
                            {
                                continue;
                            }

                            // The attribute can include a public key after a comma; compare by startswith the assembly name
                            if (!ivt.StartsWith(currentAssembly.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            accessibleConstructors++;

                            if (accessibleConstructors > 1)
                            {
                                return true;
                            }

                            break;
                        }

                        break;
                    }
                }
            }

            // If there's only one accessible constructor, do not warn
            return false;
        }
    }
}
