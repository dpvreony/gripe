// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.CodeAnalysis;

namespace Gripe.SourceGenerator
{
    /// <summary>
    /// Source generator to integrate analyzers into the dotnet tool.
    /// </summary>
    public sealed class DiagnosticCollectionFactoryGenerator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
