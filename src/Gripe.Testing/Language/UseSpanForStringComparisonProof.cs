// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForStringComparisonAnalyzer"/>.
    /// </summary>
    public static class UseSpanForStringComparisonProof
    {
        /// <summary>
        /// Proof of substring comparison that should trigger <see cref="UseSpanForStringComparisonAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStringComparisonProof.SubstringComparisonExample();
        /// </code>
        /// </example>
        public static void SubstringComparisonExample()
        {
            var text = "Hello, World!";
            var comparison = "Hello";
            _ = text.Substring(0, 5).Equals(comparison);
        }

        /// <summary>
        /// Proof of substring comparison with ordinal that should trigger <see cref="UseSpanForStringComparisonAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStringComparisonProof.SubstringComparisonOrdinalExample();
        /// </code>
        /// </example>
        public static void SubstringComparisonOrdinalExample()
        {
            var text = "Hello, World!";
            var comparison = "HELLO";
            _ = text.Substring(0, 5).Equals(comparison, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
