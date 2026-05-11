// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForUnsafeMemoryAnalyzer"/>.
    /// </summary>
    public static class UseSpanForUnsafeMemoryProof
    {
        /// <summary>
        /// Proof of fixed statement usage that should trigger <see cref="UseSpanForUnsafeMemoryAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForUnsafeMemoryProof.FixedStatementExample();
        /// </code>
        /// </example>
        public static unsafe void FixedStatementExample()
        {
            var data = new int[] { 1, 2, 3, 4, 5 };
            fixed (int* ptr = data)
            {
                _ = *ptr;
            }
        }

        /// <summary>
        /// Proof of fixed statement with byte array that should trigger <see cref="UseSpanForUnsafeMemoryAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForUnsafeMemoryProof.FixedStatementByteArrayExample();
        /// </code>
        /// </example>
        public static unsafe void FixedStatementByteArrayExample()
        {
            var buffer = new byte[1024];
            fixed (byte* ptr = buffer)
            {
                _ = ptr[0];
            }
        }
    }
}
