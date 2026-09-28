// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    /// <summary>
    /// Analyzer proof for <see cref="Gripe.Analyzer.Analyzers.Runtime.DoNotUseJsonConstructorAnalyzer"/>.
    /// </summary>
    public static class DoNotUseJsonConstructorAnalyzerProof
    {
        /// <summary>
        /// Creates an object using a constructor marked with JsonConstructor which MUST NOT warn.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseJsonConstructorAnalyzerProof.ObjectInstationTestsThatMustNotWarn();
        /// </code>
        /// </example>
        public static void ObjectInstationTestsThatMustNotWarn()
        {
            // this should not warn as the constructor has no attribute.
            _ = new ClassWithJsonConstructor();

            // this should not warn as there is only a single constructor on the class.
            _ = new ClassWithSingleConstructor(1, "abc");
        }

        /// <summary>
        /// Creates an object using a constructor marked with JsonConstructor with parameters which should warn.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseJsonConstructorAnalyzerProof.ObjectInstationTestsThatMustWarn();
        /// </code>
        /// </example>
        public static void ObjectInstationTestsThatMustWarn()
        {
            _ = new ClassWithJsonConstructor(1, "abc");
        }
    }

    /// <summary>
    /// Class with constructors marked with <c>JsonConstructor</c> to act as proof for the analyzer.
    /// </summary>
    public class ClassWithJsonConstructor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWithJsonConstructor"/> class.
        /// </summary>
        public ClassWithJsonConstructor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWithJsonConstructor"/> class with parameters.
        /// </summary>
        /// <param name="x">An integer parameter.</param>
        /// <param name="y">A string parameter.</param>
        [System.Text.Json.Serialization.JsonConstructor]
        public ClassWithJsonConstructor(int x, string y)
        {
        }
    }

    /// <summary>
    /// Class with a single constructor marked with <c>JsonConstructor</c> to act as proof for the analyzer.
    /// </summary>
    public class ClassWithSingleConstructor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWithSingleConstructor"/> class with parameters.
        /// </summary>
        /// <param name="x">An integer parameter.</param>
        /// <param name="y">A string parameter.</param>
        [System.Text.Json.Serialization.JsonConstructor]
        public ClassWithSingleConstructor(int x, string y)
        {
        }
    }

    /// <summary>
    /// Class without any <c>JsonConstructor</c> attributes.
    /// </summary>
    public class ClassWithoutJsonConstructor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWithoutJsonConstructor"/> class.
        /// </summary>
        public ClassWithoutJsonConstructor()
        {
        }
    }
}
