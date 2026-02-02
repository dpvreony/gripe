using Gripe.Analyzer.Analyzers.Abstractions;
using Gripe.Analyzer.CodeCracker.Extensions;
using Microsoft.CodeAnalysis;

namespace Gripe.Analyzer.Analyzers.Runtime
{
    /// <summary>
    /// Analyzer to detect usage of Get methods on System.IO.Directory and suggest using Enumerate methods instead.
    /// </summary>
    [Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DoNotUseDirectoryGetMethodsAnalyzer : BaseInvocationExpressionAnalyzer
    {
        private const string Title = "Do not use Get methods in System.IO.Directory";

        private const string MessageFormat = Title;

        private const string Category = SupportedCategories.Performance;

        private const string Description =
            "\"Get\" methods in System.IO.Directory can lead to performance issues when there are a large number of entries. Use \"Enumerate\" methods instead.";

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
        protected override string[] MethodNames => [
            "GetDirectories",
            "GetFiles",
            "GetFileSystemEntries"
        ];

        /// <inheritdoc />
        protected override string[] ContainingTypes => [
            "System.IO.Directory",
            "System.IO.Abstractions.IDirectory"
        ];
    }
}
