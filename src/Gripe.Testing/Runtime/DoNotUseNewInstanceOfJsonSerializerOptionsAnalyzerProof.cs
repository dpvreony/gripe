// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text.Json;
using Gripe.Analyzer.Analyzers.Runtime;

namespace Gripe.Testing.Runtime
{
    /// <summary>
    /// Analyzer proof for <see cref="DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzer"/>.
    /// </summary>
    public static class DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerProof
    {
        /// <summary>
        /// Test method that creates a new instance of <see cref="JsonSerializerOptions"/> raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerProof.SomeMethodThatCreatesNewOptions();
        /// </code>
        /// </example>
        public static void SomeMethodThatCreatesNewOptions()
        {
            _ = new JsonSerializerOptions();
        }

        /// <summary>
        /// Test method that creates a new instance of <see cref="JsonSerializerOptions"/> with defaults raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseNewInstanceOfJsonSerializerOptionsAnalyzerProof.SomeMethodThatCreatesNewOptionsWithDefaults();
        /// </code>
        /// </example>
        public static void SomeMethodThatCreatesNewOptionsWithDefaults()
        {
            _ = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }
    }
}
