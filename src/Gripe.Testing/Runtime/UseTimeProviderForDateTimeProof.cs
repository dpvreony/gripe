// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    /// <summary>Proof source for UseTimeProviderForDateTime analyzer.</summary>
    public static class UseTimeProviderForDateTimeProof
    {
        /// <summary>
        /// Reads the system clock through static date and time properties.
        /// </summary>
        /// <example>
        /// <code>
        /// UseTimeProviderForDateTimeProof.ReadSystemClock();
        /// </code>
        /// </example>
        /// <remarks>
        /// This code is just a proof for
        /// 1) making sure the code builds
        /// 2) making sure the analyzer triggers
        ///
        /// It is in no way meant to be regarded as usable code.
        /// </remarks>
        public static void ReadSystemClock()
        {
            _ = global::System.DateTime.Now;
            _ = global::System.DateTime.UtcNow;
            _ = global::System.DateTime.Today;
            _ = global::System.DateTimeOffset.Now;
            _ = global::System.DateTimeOffset.UtcNow;
            _ = global::System.DateTime.MinValue;
        }
    }
}
