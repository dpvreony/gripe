// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Gripe.UnitTests.Analyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Gripe.UnitTests.Analyzer.Verifiers
{
    /// <summary>
    /// Superclass of all Unit Tests for DiagnosticAnalyzers.
    /// </summary>
    public abstract partial class DiagnosticVerifier
    {
        /// <summary>
        /// Get the CSharp analyzer being tested - to be implemented in non-abstract class.
        /// </summary>
        /// <returns>C# Analyzer being tested.</returns>
        protected virtual DiagnosticAnalyzer? GetCSharpDiagnosticAnalyzer()
        {
            return null;
        }

        /// <summary>
        /// Get the Visual Basic analyzer being tested - to be implemented in non-abstract class.
        /// </summary>
        /// <returns>VB Analyzer being tested.</returns>
        protected virtual DiagnosticAnalyzer? GetBasicDiagnosticAnalyzer()
        {
            return null;
        }

        /// <summary>
        /// Called to test a C# DiagnosticAnalyzer when applied on the single inputted string as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected.
        /// </summary>
        /// <param name="source">A class in the form of a string to run the analyzer on.</param>
        /// <param name="expected"> DiagnosticResults that should appear after the analyzer is run on the source.</param>
        protected void VerifyCSharpDiagnostic(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(new[] { source }, LanguageNames.CSharp, GetCSharpDiagnosticAnalyzer()!, expected);
        }

        /// <summary>
        /// Called to test a VB DiagnosticAnalyzer when applied on the single inputted string as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected.
        /// </summary>
        /// <param name="source">A class in the form of a string to run the analyzer on.</param>
        /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the source.</param>
        protected void VerifyBasicDiagnostic(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(new[] { source }, LanguageNames.VisualBasic, GetBasicDiagnosticAnalyzer()!, expected);
        }

        /// <summary>
        /// Called to test a C# DiagnosticAnalyzer when applied on the inputted strings as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected.
        /// </summary>
        /// <param name="sources">An array of strings to create source documents from to run the analyzers on.</param>
        /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources.</param>
        protected void VerifyCSharpDiagnostic(string[] sources, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(sources, LanguageNames.CSharp, GetCSharpDiagnosticAnalyzer()!, expected);
        }

        /// <summary>
        /// Called to test a VB DiagnosticAnalyzer when applied on the inputted strings as a source
        /// Note: input a DiagnosticResult for each Diagnostic expected.
        /// </summary>
        /// <param name="sources">An array of strings to create source documents from to run the analyzers on.</param>
        /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources.</param>
        protected void VerifyBasicDiagnostic(string[] sources, DiagnosticResult[] expected)
        {
            VerifyDiagnostics(sources, LanguageNames.VisualBasic, GetBasicDiagnosticAnalyzer()!, expected);
        }

        /// <summary>
        /// Checks each of the actual Diagnostics found and compares them with the corresponding DiagnosticResult in the array of expected results.
        /// Diagnostics are considered equal only if the DiagnosticResultLocation, Id, Severity, and Message of the DiagnosticResult match the actual diagnostic.
        /// </summary>
        /// <param name="actualResults">The Diagnostics found by the compiler after running the analyzer on the source code.</param>
        /// <param name="analyzer">The analyzer that was being run on the sources.</param>
        /// <param name="expectedResults">Diagnostic Results that should have appeared in the code.</param>
        private static void VerifyDiagnosticResults(IEnumerable<Diagnostic> actualResults, DiagnosticAnalyzer analyzer, DiagnosticResult[] expectedResults)
        {
            var actualResultsAsArray = actualResults.ToArray();

            Assert.Multiple(
                () => Assert.All(
                    actualResultsAsArray,
                    diagnostic =>
                    {
                        var matchedDiagnostic = expectedResults.SingleOrDefault(d => d.Id == diagnostic.Id);
                        if (matchedDiagnostic == null)
                        {
                            Assert.Fail($"Unexpected diagnostic id: {diagnostic.Id}\n{FormatDiagnostics(analyzer, diagnostic)}");
                        }

                        Assert.Multiple(
                            () => Assert.Equal(matchedDiagnostic.Message, diagnostic.GetMessage(CultureInfo.InvariantCulture)),
                            () => Assert.Equal(matchedDiagnostic.Severity, diagnostic.Severity),
                            () =>
                            {
                                var actualSpan = diagnostic.Location.GetLineSpan();

                                if (!actualSpan.HasMappedPath)
                                {
                                    return;
                                }

                                var matchedFile = matchedDiagnostic.FileLocations.FirstOrDefault(fl => fl.Path == actualSpan.Path);

                                Assert.NotNull(matchedFile);

                                var lines = matchedFile.LineDetails;

                                var lineMatch = lines.SingleOrDefault(ld => ld.Line == actualSpan.StartLinePosition.Line && ld.Column == actualSpan.StartLinePosition.Character);

                                Assert.NotNull(lineMatch);

                                lineMatch.MatchCount.Increment();
                            }
                        );
                    }),

                () =>
                    {
                        var unmatched = expectedResults.Where(er => er.FileLocations.All(fl => fl.LineDetails.All(ld => ld.MatchCount.Value == 0)));
                        Assert.Empty(unmatched);
                    },

                () =>
                    {
                        var duplicates = expectedResults.Where(er => er.FileLocations.All(fl => fl.LineDetails.All(ld => ld.MatchCount.Value > 1)));
                        Assert.Empty(duplicates);
                    }
                );
        }

        /// <summary>
        /// Helper method to format a Diagnostic into an easily readable string.
        /// </summary>
        /// <param name="analyzer">The analyzer that this verifier tests.</param>
        /// <param name="diagnostics">The Diagnostics to be formatted.</param>
        /// <returns>The Diagnostics formatted as a string.</returns>
        private static string FormatDiagnostics(DiagnosticAnalyzer analyzer, params Diagnostic[] diagnostics)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < diagnostics.Length; ++i)
            {
                builder.AppendLine("// " + diagnostics[i].ToString());

                var analyzerType = analyzer.GetType();
                var rules = analyzer.SupportedDiagnostics;

                foreach (var rule in rules)
                {
                    if (rule != null && rule.Id == diagnostics[i].Id)
                    {
                        var location = diagnostics[i].Location;
                        if (location == Location.None)
                        {
                            builder.AppendFormat(CultureInfo.InvariantCulture, "GetGlobalResult({0}.{1})", analyzerType.Name, rule.Id);
                        }
                        else
                        {
                            Assert.True(
                                location.IsInSource,
                                $"Test base does not currently handle diagnostics in metadata locations. Diagnostic in metadata: {diagnostics[i]}\r\n");

                            string resultMethodName = diagnostics[i].Location.SourceTree!.FilePath.EndsWith(".cs", StringComparison.Ordinal) ? "GetCSharpResultAt" : "GetBasicResultAt";
                            var linePosition = diagnostics[i].Location.GetLineSpan().StartLinePosition;

                            builder.AppendFormat(
                                CultureInfo.InvariantCulture,
                                "{0}({1}, {2}, {3}.{4})",
                                resultMethodName,
                                linePosition.Line + 1,
                                linePosition.Character + 1,
                                analyzerType.Name,
                                rule.Id);
                        }

                        if (i != diagnostics.Length - 1)
                        {
                            builder.Append(',');
                        }

                        builder.AppendLine();
                        break;
                    }
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// General method that gets a collection of actual diagnostics found in the source after the analyzer is run,
        /// then verifies each of them.
        /// </summary>
        /// <param name="sources">An array of strings to create source documents from to run the analyzers on.</param>
        /// <param name="language">The language of the classes represented by the source strings.</param>
        /// <param name="analyzer">The analyzer to be run on the source code.</param>
        /// <param name="expected">DiagnosticResults that should appear after the analyzer is run on the sources.</param>
        private static void VerifyDiagnostics(string[] sources, string language, DiagnosticAnalyzer analyzer, params DiagnosticResult[] expected)
        {
            var diagnostics = Helpers.DiagnosticVerifier.GetSortedDiagnostics(sources, language, analyzer);
            VerifyDiagnosticResults(diagnostics, analyzer, expected);
        }
    }
}
