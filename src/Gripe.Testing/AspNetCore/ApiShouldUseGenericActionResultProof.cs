// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.AspNetCore
{
    /// <summary>Proof source for ApiShouldUseGenericActionResult analyzer.</summary>
    public sealed class ApiShouldUseGenericActionResultProof : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        /// <summary>
        /// Method returning <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.Get();
        /// </code>
        /// </example>
        public Microsoft.AspNetCore.Mvc.IActionResult? Get()
        {
            return null;
        }

        /// <summary>
        /// Method returning non-generic ActionResult.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGet();
        /// </code>
        /// </example>
        public Microsoft.AspNetCore.Mvc.ActionResult? ActionResultGet()
        {
            return null;
        }

        /// <summary>
        /// Method returning generic ActionResult directly.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetWithInt();
        /// </code>
        /// </example>
        public Microsoft.AspNetCore.Mvc.ActionResult<int>? ActionResultGetWithInt()
        {
            return null;
        }

        /// <summary>
        /// Method returning generic ActionResult directly.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetWithInt();
        /// </code>
        /// </example>
        public Microsoft.AspNetCore.Mvc.ActionResult<int> ActionResultGetWithInt(int id)
        {
            return 1;
        }

        /// <summary>
        /// Method returning generic ActionResult directly.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetWithInt();
        /// </code>
        /// </example>
        public Microsoft.AspNetCore.Mvc.ActionResult<int?> ActionResultGetWithNullableInt(int id)
        {
            return 1;
        }

        /// <summary>
        /// Method returning generic ActionResult asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetAsync();
        /// </code>
        /// </example>
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<int>>? ActionResultGetAsync()
        {
            return null;
        }

        /// <summary>
        /// Method returning generic ActionResult asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetAsync();
        /// </code>
        /// </example>
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<int>> ActionResultGetAsync(int id)
        {
            return 1;
        }

        /// <summary>
        /// Method returning generic ActionResult asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.ActionResultGetAsync();
        /// </code>
        /// </example>
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<int?>> ActionResultGetNullableIntAsync(int id)
        {
            return 1;
        }

        private void SomeProtectedMethod(int id)
        {
        }
    }
}
