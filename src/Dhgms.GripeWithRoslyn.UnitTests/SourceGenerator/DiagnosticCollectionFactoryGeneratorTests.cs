// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Gripe.SourceGenerator;
using Xunit.Abstractions;

namespace Dhgms.GripeWithRoslyn.UnitTests.SourceGenerator
{
    /// <summary>
    /// Unit Tests for <see cref="DiagnosticCollectionFactoryGenerator"/>.
    /// </summary>
    public sealed class DiagnosticCollectionFactoryGeneratorTests : AbstractGeneratorTest<DiagnosticCollectionFactoryGenerator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticCollectionFactoryGeneratorTests"/> class.
        /// </summary>
        /// <param name="output">Test Output Helper.</param>
        public DiagnosticCollectionFactoryGeneratorTests(ITestOutputHelper output)
            : base(output)
        {
        }

        /// <inheritdoc/>
        protected override string[] GetExpectedSources()
        {
            return
            [
                "DiagnosticAnalyzerCollectionFactory.g.cs",
            ];
        }
    }
}
