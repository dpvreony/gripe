// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace System.Web.Http
{
    public interface IHttpActionResult
    {
    }
}

namespace System.System.Web.Http
{
    public class ApiController
    {
    }
}

namespace Gripe.Testing.AspNetCore
{
    public sealed class ApiShouldUseGenericActionResultProof : System.System.Web.Http.ApiController
    {
        /// <summary>
        /// Method returning legacy <see cref="System.Web.Http.IHttpActionResult"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new ApiShouldUseGenericActionResultProof();
        /// _ = proof.Get();
        /// </code>
        /// </example>
        public System.Web.Http.IHttpActionResult? Get()
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
    }
}
