// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    public sealed class DoNotUseObjectAsReturnTypeProof
    {
        /// <summary>
        /// Method returning <see cref="object"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsReturnTypeProof();
        /// _ = proof.MethodName();
        /// </code>
        /// </example>
        public object? MethodName()
        {
            return null;
        }

#pragma warning disable SA1121 // Use built-in type alias
        /// <summary>
        /// Method returning <see cref="object"/> using the full type name.
        /// </summary>
        /// <param name="arg">Argument.</param>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsReturnTypeProof();
        /// _ = proof.MethodName2(new object());
        /// </code>
        /// </example>
        public System.Object? MethodName2(System.Object arg)
#pragma warning restore SA1121 // Use built-in type alias
        {
            return null;
        }
    }
}
