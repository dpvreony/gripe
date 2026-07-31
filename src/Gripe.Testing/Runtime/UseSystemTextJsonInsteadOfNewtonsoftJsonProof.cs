// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    public sealed class UseSystemTextJsonInsteadOfNewtonsoftJsonProof
    {
        /// <summary>
        /// Method using Newtonsoft.Json converter.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new UseSystemTextJsonInsteadOfNewtonsoftJsonProof();
        /// proof.MethodName();
        /// </code>
        /// </example>
        public void MethodName()
        {
            _ = new global::Newtonsoft.Json.JsonSerializer();
        }
    }
}
