// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gripe.Analyzer.Analyzers.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gripe.Testing.EfCore
{
    /// <summary>
    /// Analyzer Proofs for <see cref="MethodShouldNotReturnEntityFrameworkCoreEntityAnalyzer"/>.
    /// </summary>
    public static class ReturnsEntityProof
    {
        /// <summary>
        /// Proof of a method returning an entity.
        /// </summary>
        /// <param name="dbContext">Identity Db Context instance.</param>
        /// <returns>An entity.</returns>
        /// <example>
        /// <code>
        /// var instance = new IdentityDbContext();
        /// ReturnsEntityProof.ReturnsEntity(instance);
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer triggers
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static IdentityUser ReturnsEntity(IdentityDbContext dbContext)
        {
            return dbContext.Users.First();
        }

        /// <summary>
        /// Proof of a method returning a task of a list of entities.
        /// </summary>
        /// <param name="dbContext">Identity Db Context instance.</param>
        /// <returns>A task of entities.</returns>
        /// <example>
        /// <code>
        /// var instance = new IdentityDbContext();
        /// ReturnsEntityProof.ReturnsTaskOfEntityList(instance);
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer triggers
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static Task<List<IdentityUser>> ReturnsTaskOfEntityList(IdentityDbContext dbContext)
        {
            return dbContext.Users.ToListAsync();
        }

        /// <summary>
        /// Proof of a method returning a projection, which should not trigger the analyzer.
        /// </summary>
        /// <param name="dbContext">Identity Db Context instance.</param>
        /// <returns>A list of projected values.</returns>
        /// <example>
        /// <code>
        /// var instance = new IdentityDbContext();
        /// ReturnsEntityProof.ReturnsProjection(instance);
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer does not trigger
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static List<string?> ReturnsProjection(IdentityDbContext dbContext)
        {
            return dbContext.Users.Select(static x => x.UserName).ToList();
        }
    }
}
