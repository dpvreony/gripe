// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    /// <summary>Proof source for DoNotUseSystemConsole analyzer.</summary>
    public sealed class DoNotUseSystemConsoleProof
    {
        /// <summary>
        /// Method that writes to the console.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseSystemConsoleProof();
        /// proof.MethodName();
        /// </code>
        /// </example>
        public void MethodName()
        {
            System.Console.Write("sometext");
        }
    }
}
