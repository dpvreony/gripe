// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.Runtime.Counters;

namespace Gripe.UnitTests.Analyzer.Helpers
{
    /// <summary>
    /// Represents the line detail of a Diagnostic result.
    /// </summary>
    public sealed class DiagnosticResultLineDetail
    {
        private IncrementOnlyInt matchCount = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticResultLineDetail"/> class with the specified line and
        /// column.
        /// </summary>
        /// <param name="line">The line number, or <c>-1</c> when no line is specified.</param>
        /// <param name="column">The column number, or <c>-1</c> when no column is specified.</param>
        public DiagnosticResultLineDetail(int line, int column)
        {
            if (line < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(line), "line must be >= -1");
            }
            if (column < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "column must be >= -1");
            }
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the line of the Diagnostic result.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column of the Diagnostic result.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets the match count for the diagnostic.
        /// </summary>
        public int MatchCount => matchCount.Value;

        /// <summary>
        /// Increments the match count.
        /// </summary>
        public void IncrementMatchCount()
        {
            matchCount.Increment();
        }
    }
}
