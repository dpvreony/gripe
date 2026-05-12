// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer proofs for <see cref="AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesAnalyzer"/>.
    /// </summary>
    public static class AbstractClassesWithoutMethodImplementationsShouldProbablyBeInterfacesProof
    {
        public abstract class TypeNameWithoutMethodImplementation
        {
        }

        public abstract class TypeNameWithMethodImplementation
        {
            public void DoWork()
            {
            }
        }

        public abstract class TypeNameWithAutoProperty
        {
            public int Value { get; set; }
        }
    }
}
