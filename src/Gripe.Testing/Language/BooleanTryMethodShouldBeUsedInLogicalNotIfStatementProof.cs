// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Language
{
    public sealed class BooleanTryMethodShouldBeUsedInLogicalNotIfStatementProof
    {
        /// <summary>
        /// Method invoking <see cref="int.TryParse(string?, out int)"/> without checking its boolean result.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new BooleanTryMethodShouldBeUsedInLogicalNotIfStatementProof();
        /// proof.MethodName();
        /// </code>
        /// </example>
        public void MethodName()
        {
            int.TryParse("x", out var result);
        }
    }
}
