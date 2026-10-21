using System;
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(global::System.InvalidOperationException))]

namespace Gripe.Testing.TypeForwarding
{
    /// <summary>
    /// Proofs for type forwarding usages.
    /// </summary>
    public static class TypeBeingForwardedProof
    {
        /// <summary>
        /// Demonstrates a forwarded type used as a base type.
        /// </summary>
        /// <example>
        /// <code>
        /// var d = new Derived();
        /// </code>
        /// </example>
        public class Derived : System.InvalidOperationException
        {
            /// <inheritdoc />
            public Derived()
            {
            }

            /// <inheritdoc />
            public Derived(string message) : base(message)
            {
                // Add any type-specific logic.
            }

            /// <inheritdoc />
            public Derived(string message, Exception innerException) :
                base(message, innerException)
            {
                // Add any type-specific logic for inner exceptions.
            }
        }

        /// <summary>
        /// Demonstrates a forwarded type used as a parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <example>
        /// <code>
        /// var p = new ForwardedType();
        /// TypeBeingForwardedProof.UsesParam(p);
        /// </code>
        /// </example>
        public static void UsesParam(System.InvalidOperationException param)
        {
        }

        /// <summary>
        /// Demonstrates a forwarded type being instantiated.
        /// </summary>
        /// <example>
        /// <code>
        /// var p = new ForwardedType();
        /// </code>
        /// </example>
        public static void CreatesInstance()
        {
            var instance = new System.InvalidOperationException();
        }
    }
}
