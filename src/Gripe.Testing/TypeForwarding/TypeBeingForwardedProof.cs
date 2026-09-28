using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(global::System.InvalidOperationException))]

namespace Gripe.Testing.TypeForwarding
{
    /// <summary>
    /// Proofs for type forwarding usages.
    /// </summary>
    public class TypeBeingForwardedProof
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
        public void CreatesInstance()
        {
            var instance = new System.InvalidOperationException();
        }
    }
}
