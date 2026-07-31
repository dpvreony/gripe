// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Gripe.Testing.Language
{
    /// <summary>Proof source for MethodsThatUseReturnYieldShouldHaveNameThatBeginsWithEnumerate analyzer.</summary>
    public sealed class MethodsThatUseReturnYieldShouldHaveNameThatBeginsWithEnumerateProof
    {
        /// <summary>
        /// Method yielding values with a non-enumerate name.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new MethodsThatUseReturnYieldShouldHaveNameThatBeginsWithEnumerateProof();
        /// foreach (var item in proof.MethodName())
        /// {
        /// }
        /// </code>
        /// </example>
        public IEnumerable<int> MethodName()
        {
            yield return 1;
        }
    }
}
