// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Language
{
    /// <summary>Proof source for DoNotUseDynamicKeyword analyzer.</summary>
    public sealed class DoNotUseDynamicKeywordProof
    {
        /// <summary>
        /// Method using a dynamic parameter.
        /// </summary>
        /// <param name="arg">Dynamic argument.</param>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseDynamicKeywordProof();
        /// proof.MethodName("test");
        /// </code>
        /// </example>
        public void MethodName(dynamic arg)
        {
        }
    }
}
