// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    /// <summary>Proof source for DoNotUseObjectAsLocalVariableType analyzer.</summary>
    public sealed class DoNotUseObjectAsLocalVariableTypeProof
    {
        /// <summary>
        /// Method using <see cref="object"/> local variable.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsLocalVariableTypeProof();
        /// proof.MethodName();
        /// </code>
        /// </example>
        public void MethodName()
        {
            object name = new object();
        }

#pragma warning disable SA1121 // Use built-in type alias
        /// <summary>
        /// Method using fully qualified <see cref="object"/> local variable type.
        /// </summary>
        /// <param name="arg">Argument.</param>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsLocalVariableTypeProof();
        /// proof.MethodName2(new object());
        /// </code>
        /// </example>
        public void MethodName2(int arg)
#pragma warning restore SA1121 // Use built-in type alias
        {
            System.Object name = new object();
        }

        /// <summary>
        /// Method using implicit var with object initializer.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsLocalVariableTypeProof();
        /// proof.MethodName3();
        /// </code>
        /// </example>
        public void MethodName3()
        {
            var name = new object();
        }

        /// <summary>
        /// Method using implicit var with fully qualified object initializer.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsLocalVariableTypeProof();
        /// proof.MethodName4();
        /// </code>
        /// </example>
        public void MethodName4()
        {
            var name = new System.Object();
        }

        /// <summary>
        /// Method using implicit var with fully qualified object initializer.
        /// </summary>
        /// <example>
        /// <code>
        /// var proof = new DoNotUseObjectAsLocalVariableTypeProof();
        /// proof.MethodName4();
        /// </code>
        /// </example>
        public void MethodName5()
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            object? name = null;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
        }
    }
}
