// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Abstractions;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;

namespace Gripe.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Analyzer to warn against creating new instances of JsonSerializerOptions.
    /// </summary>
    [Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer : BaseObjectCreationUsingTypeAnalyzer
    {
        internal const string Title = "Do not use new instances of JsonSerializerOptions. Reuse existing instances for better performance.";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Performance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer"/> class.
        /// </summary>
        public DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer()
            : base(
            DiagnosticIdsHelper.DoNotUseNewInstanceOfJsonSerializerOptions,
            Title,
            MessageFormat,
            Category,
            DiagnosticResultDescriptionFactory.DoNotUseNewInstanceOfJsonSerializerOptions(),
            DiagnosticSeverity.Warning)
        {
        }

        /// <inheritdoc />
        protected override string FullTypeName => "global::System.Text.Json.JsonSerializerOptions";
    }
}
