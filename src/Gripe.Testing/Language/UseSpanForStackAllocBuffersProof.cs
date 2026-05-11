// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForStackAllocBuffersAnalyzer"/>.
    /// </summary>
    public static class UseSpanForStackAllocBuffersProof
    {
        /// <summary>
        /// Proof of small byte array creation that should trigger <see cref="UseSpanForStackAllocBuffersAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStackAllocBuffersProof.SmallByteArrayExample();
        /// </code>
        /// </example>
        public static void SmallByteArrayExample()
        {
            _ = new byte[256];
        }

        /// <summary>
        /// Proof of small byte array creation at threshold that should trigger <see cref="UseSpanForStackAllocBuffersAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStackAllocBuffersProof.ThresholdByteArrayExample();
        /// </code>
        /// </example>
        public static void ThresholdByteArrayExample()
        {
            _ = new byte[512];
        }

        /// <summary>
        /// Proof of large byte array creation that should not trigger <see cref="UseSpanForStackAllocBuffersAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForStackAllocBuffersProof.LargeByteArrayExample();
        /// </code>
        /// </example>
        public static void LargeByteArrayExample()
        {
            _ = new byte[1024];
        }
    }
}
