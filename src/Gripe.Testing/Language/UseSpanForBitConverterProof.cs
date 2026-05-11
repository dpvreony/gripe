// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForBitConverterAnalyzer"/>.
    /// </summary>
    public static class UseSpanForBitConverterProof
    {
        /// <summary>
        /// Proof of BitConverter.ToInt32 usage that should trigger <see cref="UseSpanForBitConverterAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForBitConverterProof.BitConverterToInt32Example();
        /// </code>
        /// </example>
        public static void BitConverterToInt32Example()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            _ = BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Proof of BitConverter.GetBytes usage that should trigger <see cref="UseSpanForBitConverterAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForBitConverterProof.BitConverterGetBytesExample();
        /// </code>
        /// </example>
        public static void BitConverterGetBytesExample()
        {
            _ = BitConverter.GetBytes(12345);
        }

        /// <summary>
        /// Proof of BitConverter.ToDouble usage that should trigger <see cref="UseSpanForBitConverterAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForBitConverterProof.BitConverterToDoubleExample();
        /// </code>
        /// </example>
        public static void BitConverterToDoubleExample()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            _ = BitConverter.ToDouble(bytes, 0);
        }
    }
}
