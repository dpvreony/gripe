// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer;
using Gripe.Analyzer.Analyzers.Logging;
using Gripe.UnitTests.Analyzer.Verifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;
using DiagnosticResult = Gripe.UnitTests.Analyzer.Helpers.DiagnosticResult;
using DiagnosticResultLocation = Gripe.UnitTests.Analyzer.Helpers.DiagnosticResultLocation;

namespace Gripe.UnitTests.Analyzer.Analyzers.Logging
{
    /// <summary>
    /// Unit tests for the <see cref="DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer"/> class.
    /// </summary>
    public sealed class DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzerTest : CodeFixVerifier
    {
        /// <summary>
        /// Test to ensure good code doesn't return a warning.
        /// </summary>
        [Fact]
        public void ReturnsNoWarnings()
        {
            var test = @"
    #nullable enable
    namespace Microsoft.Extensions.Logging
    {
        public enum LogLevel
        {
            Error
        }

        public readonly struct EventId
        {
        }

        public interface ILogger
        {
        }

        public static class LoggerExtensions
        {
            public static void LogError(this ILogger logger, System.Exception? exception, string message, params object[] args)
            {
            }

            public static void LogWarning(this ILogger logger, System.Exception? exception, string message, params object[] args)
            {
            }

            public static void LogWarning(this ILogger logger, string message, params object[] args)
            {
            }

            public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, object state, System.Exception? exception, System.Func<object, System.Exception?, string> formatter)
            {
            }
        }
    }

    namespace ConsoleApplication1
    {
        using Microsoft.Extensions.Logging;

        public sealed class TypeUnderTest
        {
            private readonly ILogger _logger;

            public TypeUnderTest(ILogger logger)
            {
                _logger = logger;
            }

            public void LogThings(System.Exception exception, System.InvalidOperationException invalidOperationException)
            {
                _logger.LogError(exception, ""Oops"");
                _logger.LogWarning(exception: invalidOperationException, message: ""Oops {Arg}"", args: new object[] { 1 });
                _logger.Log(LogLevel.Error, new EventId(), ""state"", exception, (state, loggedException) => state.ToString() ?? string.Empty);
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        /// <summary>
        /// Test to ensure bad code returns a warning.
        /// </summary>
        [Fact]
        public void ReturnsWarning()
        {
            var test = @"
    #nullable enable
    namespace Microsoft.Extensions.Logging
    {
        public enum LogLevel
        {
            Error
        }

        public readonly struct EventId
        {
        }

        public interface ILogger
        {
        }

        public static class LoggerExtensions
        {
            public static void LogError(this ILogger logger, System.Exception? exception, string message, params object[] args)
            {
            }

            public static void LogError(this ILogger logger, string message, params object[] args)
            {
            }

            public static void LogWarning(this ILogger logger, System.Exception? exception, string message, params object[] args)
            {
            }

            public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, object state, System.Exception? exception, System.Func<object, System.Exception?, string> formatter)
            {
            }
        }
    }

    namespace ConsoleApplication1
    {
        using Microsoft.Extensions.Logging;

        public sealed class TypeUnderTest
        {
            private readonly ILogger _logger;

            public TypeUnderTest(ILogger logger)
            {
                _logger = logger;
            }

            public void LogThings(System.Exception exception, System.InvalidOperationException invalidOperationException)
            {
                _logger.LogError(""Oops"", exception);
                _logger.LogWarning(""Oops {Arg}"", exception);
                _logger.Log(LogLevel.Error, new EventId(), exception, null, (state, loggedException) => state.ToString() ?? string.Empty);
            }
        }
    }";

            var expected = new[]
            {
                new DiagnosticResult
                {
                    Id = DiagnosticIdsHelper.DoNotPassExceptionIntoNonExceptionLoggingArgument,
                    Message = DiagnosticResultTitleFactory.DoNotPassExceptionIntoNonExceptionLoggingArgument(),
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 53, 42),
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticIdsHelper.DoNotPassExceptionIntoNonExceptionLoggingArgument,
                    Message = DiagnosticResultTitleFactory.DoNotPassExceptionIntoNonExceptionLoggingArgument(),
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 54, 50),
                        }
                },
                new DiagnosticResult
                {
                    Id = DiagnosticIdsHelper.DoNotPassExceptionIntoNonExceptionLoggingArgument,
                    Message = DiagnosticResultTitleFactory.DoNotPassExceptionIntoNonExceptionLoggingArgument(),
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[]
                        {
                            new DiagnosticResultLocation("Test0.cs", 55, 60),
                        }
                },
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        /// <inheritdoc />
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer();
        }
    }
}
