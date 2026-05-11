// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForSubstringAnalyzer"/>.
    /// </summary>
    public static class UseSpanForSubstringProof
    {
        /// <summary>
        /// Proof of Substring usage that should trigger <see cref="UseSpanForSubstringAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForSubstringProof.SubstringExample();
        /// </code>
        /// </example>
        public static void SubstringExample()
        {
            var text = "Hello, World!";
            _ = text.Substring(0, 5);
        }

        /// <summary>
        /// Proof of Substring with single parameter usage that should trigger <see cref="UseSpanForSubstringAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForSubstringProof.SubstringSingleParameterExample();
        /// </code>
        /// </example>
        public static void SubstringSingleParameterExample()
        {
            var text = "Hello, World!";
            _ = text.Substring(7);
        }
    }
}
