// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Gripe.Analyzer.Analyzers.Language;

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer proofs for <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
    /// </summary>
    public static class ConsiderValueTaskInsteadOfTaskProof
    {
        private static readonly Task<int> CachedTask = Task.FromResult(0);

        /// <summary>
        /// Positive proof: private non-async expression-bodied method returning Task.FromResult.
        /// Should trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        private static Task<int> PrivateExpressionBodied() => Task.FromResult(42);

        /// <summary>
        /// Positive proof: private non-async branch-based fast path returning Task.FromResult on the hot path.
        /// Should trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        private static Task<string> PrivateBranchFastPath(string key, Dictionary<string, string> cache)
        {
            if (cache.TryGetValue(key, out var value))
                return Task.FromResult(value);

            return LoadFromNetworkAsync(key);
        }

        // Negative: public method - changing return type risks breaking the API contract.
        /// <summary>
        /// Negative proof: public method where the API contract change is too risky.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var result = await ConsiderValueTaskInsteadOfTaskProof.PublicMethod();
        /// </code>
        /// </example>
        public static Task<int> PublicMethod() => Task.FromResult(42);

        // Negative: returns a cached/stored Task instance which may be shared or re-awaited.
        private static Task<int> ReturnsCachedTask() => CachedTask;

        // Negative: async method with multiple awaits - ValueTask offers little benefit.
        private static async Task<int> MultipleAwaitsAsync()
        {
            var a = await Task.FromResult(1);
            var b = await Task.FromResult(2);
            return a + b;
        }

        private static async Task<string> LoadFromNetworkAsync(string key)
        {
            await Task.Delay(1);
            return key;
        }
    }

    /// <summary>
    /// Abstract base used to proof protected, abstract, and virtual method exclusions.
    /// </summary>
    public abstract class AbstractConsiderValueTaskInsteadOfTaskProofBase
    {
        // Negative: protected member - callers may depend on Task<T> semantics.
        /// <summary>
        /// Negative proof: protected method where the signature change is too risky.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        protected Task<int> ProtectedMethod() => Task.FromResult(42);

        // Negative: abstract member - no body to analyse.
        /// <summary>
        /// Negative proof: abstract method with no body to analyse.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// // Override in a derived class:
        /// public override Task&lt;int&gt; AbstractMethod() =&gt; Task.FromResult(42);
        /// </code>
        /// </example>
        public abstract Task<int> AbstractMethod();

        // Negative: virtual member - overriders may return Task<T> objects.
        /// <summary>
        /// Negative proof: virtual method where overriders may rely on Task&lt;T&gt; semantics.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// AbstractConsiderValueTaskInsteadOfTaskProofBase instance = new ConsiderValueTaskInsteadOfTaskProofOverride();
        /// var result = await instance.VirtualMethod();
        /// </code>
        /// </example>
        public virtual Task<int> VirtualMethod() => Task.FromResult(42);
    }

    /// <summary>
    /// Sealed override proof for override-based exclusions.
    /// </summary>
    public sealed class ConsiderValueTaskInsteadOfTaskProofOverride : AbstractConsiderValueTaskInsteadOfTaskProofBase
    {
        // Negative: override - must match base signature.
        /// <summary>
        /// Negative proof: override must match the base signature.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        public override Task<int> AbstractMethod() => Task.FromResult(42);

        // Negative: override - must match base signature.
        /// <summary>
        /// Negative proof: override must match the base signature.
        /// Should not trigger <see cref="ConsiderValueTaskInsteadOfTaskAnalyzer"/>.
        /// </summary>
        public override Task<int> VirtualMethod() => Task.FromResult(42);
    }
}
