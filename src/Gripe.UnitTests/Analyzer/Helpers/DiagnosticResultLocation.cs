// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Gripe.UnitTests.Analyzer.Helpers
{
    /// <summary>
    /// Location where the diagnostic appears, as determined by path, line number, and column number.
    /// </summary>
    /// <param name="Path">File path.</param>
    /// <param name="LineDetails">Line details in the file.</param>
    public record DiagnosticResultFileLocation(
        string Path,
        IReadOnlyCollection<DiagnosticResultLineDetail> LineDetails);
}