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
        /// <summary>
        /// Proof type for an abstract class without method implementations.
        /// </summary>
        public abstract class TypeNameWithoutMethodImplementation
        {
        }

        /// <summary>
        /// Proof type for an abstract class with a method implementation.
        /// </summary>
        public abstract class TypeNameWithMethodImplementation
        {
            /// <summary>
            /// Does work.
            /// </summary>
            public void DoWork()
            {
            }
        }

        /// <summary>
        /// Proof type for an abstract class with only an auto property.
        /// </summary>
        public abstract class TypeNameWithAutoProperty
        {
            /// <summary>
            /// Gets or sets a value.
            /// </summary>
            public int Value { get; set; }
        }

        /// <summary>
        /// Proof base type for demonstrating generic constraint restriction scenario.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        public class TypeNameGenericBase<T>
            where T : System.IDisposable
        {
        }

        /// <summary>
        /// Proof type for an abstract class inheriting from a generic class with restricted constraints.
        /// Should NOT be flagged by GR0065.
        /// </summary>
        /// <typeparam name="T">The type parameter.</typeparam>
        public abstract class TypeNameInheritingFromGenericClassWithConstraints<T> : TypeNameGenericBase<T>
            where T : System.IDisposable, System.IComparable
        {
        }
    }
}
