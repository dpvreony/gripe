// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Gripe.Testing.Runtime
{
    public sealed class DoNotUseObjectAsPropertyTypeProof
    {
        public object? SomeProperty => null;

#pragma warning disable SA1121 // Use built-in type alias
        public System.Object? SomeProperty2 => null;
#pragma warning restore SA1121 // Use built-in type alias
    }
}
