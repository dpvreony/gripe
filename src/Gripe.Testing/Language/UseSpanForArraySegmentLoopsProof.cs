// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForArraySegmentLoopsAnalyzer"/>.
    /// </summary>
    public static class UseSpanForArraySegmentLoopsProof
    {
        /// <summary>
        /// Proof of looping over array segments that should trigger <see cref="UseSpanForArraySegmentLoopsAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForArraySegmentLoopsProof.ArraySegmentLoopExample();
        /// </code>
        /// </example>
        public static void ArraySegmentLoopExample()
        {
            var data = new int[] { 1, 2, 3, 4, 5 };
            for (int[] segment = data; ; )
            {
                break;
            }
        }
    }
}
