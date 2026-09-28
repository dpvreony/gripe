// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Log levels for logging proof code.
    /// </summary>
    internal enum LogLevel
    {
        /// <summary>
        /// Error log level.
        /// </summary>
        Error
    }

    /// <summary>
    /// Event id for logging proof code.
    /// </summary>
    internal readonly struct EventId
    {
    }

    /// <summary>
    /// Minimal logger interface for logging proof code.
    /// </summary>
    internal interface ILogger
    {
    }

    /// <summary>
    /// Logging extension methods for proof code.
    /// </summary>
    internal static class LoggerExtensions
    {
        /// <summary>
        /// Proof logging overload with explicit exception.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The message arguments.</param>
        internal static void LogError(
            this ILogger logger,
            Exception? exception,
            string message,
            params object[] args)
        {
        }

        /// <summary>
        /// Proof logging overload with params arguments only.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The message arguments.</param>
        internal static void LogError(
            this ILogger logger,
            string message,
            params object[] args)
        {
        }

        /// <summary>
        /// Proof logging overload with explicit exception.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The message arguments.</param>
        internal static void LogWarning(
            this ILogger logger,
            Exception? exception,
            string message,
            params object[] args)
        {
        }

        /// <summary>
        /// Proof logging overload with params arguments only.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The message arguments.</param>
        internal static void LogWarning(
            this ILogger logger,
            string message,
            params object[] args)
        {
        }

        /// <summary>
        /// Proof direct log overload.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="state">The state.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="formatter">The formatter.</param>
        internal static void Log(
            this ILogger logger,
            LogLevel logLevel,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
        {
        }
    }
}

namespace Gripe.Testing.Logging
{
    using Microsoft.Extensions.Logging;

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
            Microsoft.Extensions.Logging.ILogger logger,
            Exception exception,
            InvalidOperationException invalidOperationException)
        {
            logger.LogError(exception, "Oops");
            logger.LogWarning(
                exception: invalidOperationException,
                message: "Oops {Arg}",
                args: new object[] { 1 });
            logger.Log(
                Microsoft.Extensions.Logging.LogLevel.Error,
                new Microsoft.Extensions.Logging.EventId(),
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
            Microsoft.Extensions.Logging.ILogger logger,
            Exception exception,
            InvalidOperationException invalidOperationException,
            bool includeNull)
        {
            logger.LogError("Oops", exception);
            logger.LogWarning(message: "Oops {Arg}", exception);
            logger.LogWarning("Oops {Arg}", invalidOperationException);
            logger.LogError("Oops {Arg}", includeNull ? null : exception);
            logger.Log(
                Microsoft.Extensions.Logging.LogLevel.Error,
                new Microsoft.Extensions.Logging.EventId(),
                exception,
                null,
                FormatState);
        }

        private static string FormatState(object state, Exception? exception)
        {
            _ = exception;
            return state.ToString() ?? string.Empty;
        }

        private sealed class ProofLogger : Microsoft.Extensions.Logging.ILogger
        {
        }
    }
}
