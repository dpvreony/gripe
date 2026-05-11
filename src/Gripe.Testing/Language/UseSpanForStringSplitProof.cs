// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForStringSplitAnalyzer"/>.
    /// </summary>
    public static class UseSpanForStringSplitProof
    {
        /// <summary>
        /// Proof of string.Split usage that should trigger <see cref="UseSpanForStringSplitAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStringSplitProof.StringSplitExample();
        /// </code>
        /// </example>
        public static void StringSplitExample()
        {
            var text = "apple,banana,cherry";
            _ = text.Split(',');
        }

        /// <summary>
        /// Proof of string.Split with multiple separators that should trigger <see cref="UseSpanForStringSplitAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStringSplitProof.StringSplitMultipleSeparatorsExample();
        /// </code>
        /// </example>
        public static void StringSplitMultipleSeparatorsExample()
        {
            var text = "apple,banana;cherry";
            _ = text.Split(',', ';');
        }
    }
}
