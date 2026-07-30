// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gripe.Testing.EfCore
{
    /// <summary>
    /// Analyzer Proofs for EF Core LINQ methods that should use static lambda.
    /// </summary>
    public static class EfCoreStaticLambdaProof
    {
        /// <summary>
        /// Proof of Select, OrderBy, OrderByDescending, GroupBy, ThenBy and Include invocations to trigger the analyzer.
        /// </summary>
        /// <param name="dbContext">Identity Db Context instance.</param>
        public static void CallsQueryMethods(IdentityDbContext dbContext)
        {
            _ = dbContext.Users.Select(x => x.Id).ToList();
            _ = dbContext.Users.OrderBy(x => x.UserName).ToList();
            _ = dbContext.Users.OrderByDescending(x => x.UserName).ToList();
            _ = dbContext.Users.GroupBy(x => x.UserName).ToList();
            _ = dbContext.Users.OrderBy(static x => x.UserName).ThenBy(x => x.Email).ToList();
            _ = dbContext.Users.Include(x => x.UserName).ToList();
        }
    }
}
