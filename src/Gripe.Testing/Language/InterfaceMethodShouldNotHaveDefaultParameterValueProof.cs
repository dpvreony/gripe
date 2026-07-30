// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer proofs for interface method default parameter values.
    /// </summary>
    public interface IInterfaceMethodShouldNotHaveDefaultParameterValueProof
    {
        /// <summary>
        /// Interface method with default parameter value.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="quantity">The item quantity.</param>
        /// <example>
        /// <code>
        /// IInterfaceMethodShouldNotHaveDefaultParameterValueProof proof = null;
        /// proof!.AddItem("foo");
        /// </code>
        /// </example>
        void AddItem(string name, int quantity = 2);

        /// <summary>
        /// Interface method without default parameter value.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="quantity">The item quantity.</param>
        /// <example>
        /// <code>
        /// IInterfaceMethodShouldNotHaveDefaultParameterValueProof proof = null;
        /// proof!.AddItemNoDefault("foo", 2);
        /// </code>
        /// </example>
        void AddItemNoDefault(string name, int quantity);
    }

    /// <summary>
    /// Concrete class method with default parameter value.
    /// </summary>
    public sealed class InterfaceMethodShouldNotHaveDefaultParameterValueProof
    {
        /// <summary>
        /// Class method with default parameter value.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="quantity">The item quantity.</param>
        /// <example>
        /// <code>
        /// var proof = new InterfaceMethodShouldNotHaveDefaultParameterValueProof();
        /// proof.AddItem("foo");
        /// </code>
        /// </example>
        public void AddItem(string name, int quantity = 2)
        {
        }
    }
}
