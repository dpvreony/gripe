// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForArrayCopyAnalyzer"/>.
    /// </summary>
    public static class UseSpanForArrayCopyProof
    {
        /// <summary>
        /// Proof of Array.Copy usage that should trigger <see cref="UseSpanForArrayCopyAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForArrayCopyProof.ArrayCopyExample();
        /// </code>
        /// </example>
        public static void ArrayCopyExample()
        {
            var source = new int[] { 1, 2, 3, 4, 5 };
            var destination = new int[5];
            Array.Copy(source, destination, 5);
        }

        /// <summary>
        /// Proof of Array.Clone usage that should trigger <see cref="UseSpanForArrayCopyAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForArrayCopyProof.ArrayCloneExample();
        /// </code>
        /// </example>
        public static void ArrayCloneExample()
        {
            var source = new int[] { 1, 2, 3, 4, 5 };
            _ = source.Clone();
        }
    }
}
