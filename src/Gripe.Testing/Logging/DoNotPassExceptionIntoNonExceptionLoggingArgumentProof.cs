// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Gripe.Testing.Logging
{
    /// <summary>
    /// Analyzer proof for <see cref="Gripe.Analyzer.Analyzers.Logging.DoNotPassExceptionIntoNonExceptionLoggingArgumentAnalyzer"/>.
    /// </summary>
    public static class DoNotPassExceptionIntoNonExceptionLoggingArgumentProof
    {
        /// <summary>
        /// Invokes logging overloads that should not trigger the analyzer.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="invalidOperationException">A derived exception.</param>
        /// <example>
        /// <code>
        /// var logger = new ProofLogger();
        /// var exception = new Exception("message");
        /// var invalidOperationException = new InvalidOperationException("message");
        /// DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.AllowedCalls(
        ///     logger,
        ///     exception,
        ///     invalidOperationException);
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer triggers
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static void AllowedCalls(
            ILogger logger,
            Exception exception,
            InvalidOperationException invalidOperationException)
        {
            logger.LogError(exception, "Oops");
            logger.LogWarning(
                exception: invalidOperationException,
                message: "Oops {Arg}",
                args: new object[] { 1 });
            logger.Log(
                LogLevel.Error,
                new EventId(),
                "state",
                exception,
                FormatState);
        }

        /// <summary>
        /// Invokes logging overloads that should trigger the analyzer.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="invalidOperationException">A derived exception.</param>
        /// <param name="includeNull">Determines whether to include null in a conditional expression.</param>
        /// <example>
        /// <code>
        /// var logger = new ProofLogger();
        /// var exception = new Exception("message");
        /// var invalidOperationException = new InvalidOperationException("message");
        /// DoNotPassExceptionIntoNonExceptionLoggingArgumentProof.WarningCalls(
        ///     logger,
        ///     exception,
        ///     invalidOperationException,
        ///     false);
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer triggers
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static void WarningCalls(
            ILogger logger,
            Exception exception,
            InvalidOperationException invalidOperationException,
            bool includeNull)
        {
            logger.LogError("Oops {Arg}", exception);
            logger.LogWarning(message: "Oops {Arg}", exception);
            logger.LogWarning("Oops {Arg}", invalidOperationException);
            logger.LogError("Oops {Arg}", includeNull ? null : exception);
            logger.Log(
                LogLevel.Error,
                new EventId(),
                exception,
                null,
                FormatState);
        }

        private static string FormatState(object state, Exception? exception)
        {
            _ = exception;
            return state.ToString() ?? string.Empty;
        }

        private sealed class ProofLogger : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state)
                where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
            }
        }
    }
}
