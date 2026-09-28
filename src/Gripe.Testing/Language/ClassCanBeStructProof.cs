// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Language
{
    /// <summary>
    /// Analyzer proof for <see cref="Gripe.Analyzer.Analyzers.Language.ClassCanBeStructAnalyzer"/>.
    /// </summary>
    public static class ClassCanBeStructProof
    {
        /// <summary>
        /// Creates classes that should trigger the class-to-struct analyzer.
        /// </summary>
        /// <example>
        /// <code>
        /// ClassCanBeStructProof.ClassesThatShouldWarn();
        /// </code>
        /// </example>
        public static void ClassesThatShouldWarn()
        {
            _ = new SmallImmutableClass(1, 2);
        }

        /// <summary>
        /// Creates classes that should not trigger the class-to-struct analyzer.
        /// </summary>
        /// <example>
        /// <code>
        /// ClassCanBeStructProof.ClassesThatShouldNotWarn();
        /// </code>
        /// </example>
        public static void ClassesThatShouldNotWarn()
        {
            _ = new MutableClass(1);
            _ = new ClassWithReferenceField("test");
            _ = new InheritingClass(1);
        }
    }

    /// <summary>
    /// Small immutable class that should be considered a struct candidate.
    /// </summary>
    public sealed class SmallImmutableClass
    {
        private readonly int _x;
        private readonly int _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallImmutableClass"/> class.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public SmallImmutableClass(int x, int y)
        {
            _x = x;
            _y = y;
            Consume();
        }

        private void Consume()
        {
            _ = _x + _y;
        }
    }

    /// <summary>
    /// Mutable class which should not be considered a struct candidate.
    /// </summary>
    public sealed class MutableClass
    {
        private int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableClass"/> class.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public MutableClass(int value)
        {
            _value = value;
            Consume();
        }

        private void Consume()
        {
            _ = _value;
        }
    }

    /// <summary>
    /// Class with a reference type field which should not be considered a struct candidate.
    /// </summary>
    public sealed class ClassWithReferenceField
    {
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWithReferenceField"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ClassWithReferenceField(string value)
        {
            _value = value;
            Consume();
        }

        private void Consume()
        {
            _ = _value;
        }
    }

    /// <summary>
    /// Base class used to ensure inherited classes do not trigger diagnostics.
    /// </summary>
    public class BaseProofClass
    {
    }

    /// <summary>
    /// Class inheriting from a custom base class which should not be considered a struct candidate.
    /// </summary>
    public sealed class InheritingClass : BaseProofClass
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritingClass"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public InheritingClass(int value)
        {
            _value = value;
            Consume();
        }

        private void Consume()
        {
            _ = _value;
        }
    }
}
