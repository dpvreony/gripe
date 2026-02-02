using Gripe.Analyzer.Analyzers.Abstractions;
using Gripe.Analyzer.Analyzers.EfCore;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gripe.Analyzer.Analyzers.Runtime
{
    [Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotUseDirectoryGetMethodsAnalyzer : BaseInvocationExpressionAnalyzer
    {
        private const string Title = "Do not use System.IO.Directory.GetFiles()";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Performance;

        private const string Description =
            "System.IO.Directory.GetFiles can lead to performance issues when there are a large number of files. Use System.IO.Directory.EnumerateFiles().";

        /// <summary>
        /// Initializes a new instance of the <see cref="DoNotUseDirectoryGetMethodsAnalyzer"/> class.
        /// </summary>
        public DoNotUseDirectoryGetMethodsAnalyzer()
            : base(
                DiagnosticIdsHelper.DoNotUseDirectoryGetMethods,
                Title,
                MessageFormat,
                Category,
                Description,
                DiagnosticSeverity.Warning)
        {
        }

        /// <inheritdoc />
        protected override string MethodName => "GetFiles";

        /// <inheritdoc />
        protected override string[] ContainingTypes => [
            "System.IO.Directory"
        ];
    }
}
