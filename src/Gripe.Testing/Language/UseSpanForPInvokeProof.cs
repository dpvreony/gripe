// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer Proofs for <see cref="UseSpanForPInvokeAnalyzer"/>.
    /// </summary>
    public static class UseSpanForPInvokeProof
    {
        /// <summary>
        /// Proof of P/Invoke with byte array that should trigger <see cref="UseSpanForPInvokeAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// UseSpanForPInvokeProof.PInvokeByteArrayExample();
        /// </code>
        /// </example>
        public static void PInvokeByteArrayExample()
        {
            var buffer = new byte[1024];
            _ = NativeMethods.ReadData(buffer, buffer.Length);
        }

        /// <summary>
        /// Native methods for P/Invoke examples.
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// Example P/Invoke method that reads data.
            /// </summary>
            /// <param name="buffer">The buffer to read into.</param>
            /// <param name="length">The length of data to read.</param>
            /// <returns>The number of bytes read.</returns>
            [DllImport("kernel32.dll")]
            internal static extern int ReadData(byte[] buffer, int length);
        }
    }
}
