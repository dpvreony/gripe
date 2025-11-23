// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Foundatio.Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.GripeWithRoslyn.UnitTests.SourceGenerator
{
    /// <summary>
    /// Base class for testing generators.
    /// </summary>
    /// <typeparam name="TGenerator">The type for the source generator to test.</typeparam>
    public abstract class AbstractGeneratorTest<TGenerator> : TestWithLoggingBase
        where TGenerator : IIncrementalGenerator, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGeneratorTest{TGenerator}"/> class.
        /// </summary>
        /// <param name="output">Test Output Helper.</param>
        protected AbstractGeneratorTest(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <summary>
        /// Test to ensure anaylzer returns diagnostic results.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ReturnsDiagnosticResults()
        {
            var generator = new TGenerator();

            using (var workspace = MSBuildWorkspace.Create())
            {
                // MSBuildProjectDirectory
                var project =
                    await workspace.OpenProjectAsync(
                        "../../../../Dhgms.GripeWithRoslyn.Testing/Dhgms.GripeWithRoslyn.Testing.csproj");
                var compilation = await project.GetCompilationAsync();
                if (compilation == null)
                {
                    // TODO: warn about failure to get compilation object.
                    throw new InvalidOperationException("Failed to get compilation object");
                }

                var newComp = RunGenerators(
                    compilation,
                    null,
                    out var generatorDiags,
                    generator.AsSourceGenerator());

                _logger.LogInformation($"Generator Diagnostic count : {generatorDiags.Length}");

                var hasErrors = false;
                foreach (var generatorDiag in generatorDiags)
                {
                    _logger.LogInformation(generatorDiag.ToString());

                    hasErrors |= generatorDiag.Severity == DiagnosticSeverity.Error;
                }

                foreach (var newCompSyntaxTree in newComp.SyntaxTrees)
                {
                    _logger.LogInformation("Syntax Tree:");
                    _logger.LogInformation(newCompSyntaxTree.GetText().ToString());
                }

                Assert.False(hasErrors);
            }
        }

        private static GeneratorDriver CreateDriver(
            Compilation compilation,
            AnalyzerConfigOptionsProvider? analyzerConfigOptionsProvider,
            params ISourceGenerator[] generators) => CSharpGeneratorDriver.Create(
            generators: ImmutableArray.Create(generators),
            additionalTexts: ImmutableArray<AdditionalText>.Empty,
            parseOptions: (CSharpParseOptions)compilation.SyntaxTrees.First().Options,
            optionsProvider: analyzerConfigOptionsProvider);

        private static Compilation RunGenerators(
            Compilation compilation,
            AnalyzerConfigOptionsProvider? analyzerConfigOptionsProvider,
            out ImmutableArray<Diagnostic> diagnostics,
            params ISourceGenerator[] generators)
        {
            var driver = CreateDriver(
                compilation,
                analyzerConfigOptionsProvider,
                generators);

            driver.RunGeneratorsAndUpdateCompilation(
                compilation,
                out var updatedCompilation,
                out diagnostics);

            return updatedCompilation;
        }
    }
}
