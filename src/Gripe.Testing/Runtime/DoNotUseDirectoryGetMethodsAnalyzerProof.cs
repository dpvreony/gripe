using System.IO.Abstractions.TestingHelpers;
using Gripe.Analyzer.Analyzers.Runtime;

namespace Gripe.Testing.Runtime
{
    /// <summary>
    /// Analyzer proof for <see cref="DoNotUseDirectoryGetMethodsAnalyzer"/>.
    /// </summary>
    public static class DoNotUseDirectoryGetMethodsAnalyzerProof
    {
        /// <summary>
        /// Test method that gets directories raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsDirectories();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsDirectories()
        {
            _ = System.IO.Directory.GetDirectories("C:\\SomePath");
        }

        /// <summary>
        /// Test method that gets directories via abstraction raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsDirectoriesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsDirectoriesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.GetDirectories("C:\\SomePath");
        }

        /// <summary>
        /// Test method that gets files raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsFiles();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsFiles()
        {
            _ = System.IO.Directory.GetFiles("C:\\SomePath");
        }

        /// <summary>
        /// Test method that gets files via abstraction raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsFilesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsFilesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.GetFiles("C:\\SomePath");
        }

        /// <summary>
        /// Test method that gets file system entries raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsFileSystemEntries();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsFileSystemEntries()
        {
            _ = System.IO.Directory.GetFileSystemEntries("C:\\SomePath");
        }

        /// <summary>
        /// Test method that gets file system entries via abstraction raises a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatGetsFileSystemEntriesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatGetsFileSystemEntriesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.GetFileSystemEntries("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates directories does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumeratesDirectories();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumeratesDirectories()
        {
            _ = System.IO.Directory.EnumerateDirectories("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates directories via an abstraction does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumerateDirectoriesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumerateDirectoriesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.EnumerateDirectories("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates files does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumeratesFiles();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumeratesFiles()
        {
            _ = System.IO.Directory.EnumerateFiles("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates files via an abstraction does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumeratesFilesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumeratesFilesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.EnumerateFiles("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates file system entries does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumerateFileSystemEntries();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumerateFileSystemEntries()
        {
            _ = System.IO.Directory.EnumerateFileSystemEntries("C:\\SomePath");
        }

        /// <summary>
        /// Test method that enumerates file system entries via an abstraction does not raise a diagnostic.
        /// </summary>
        /// <example>
        /// <code>
        /// DoNotUseDirectoryGetMethodsAnalyzerProof.SomeMethodThatEnumerateFileSystemEntriesViaAbstraction();
        /// </code>
        /// </example>
        public static void SomeMethodThatEnumerateFileSystemEntriesViaAbstraction()
        {
            var fileSystem = new MockFileSystem();
            _ = fileSystem.Directory.EnumerateFileSystemEntries("C:\\SomePath");
        }
    }
}
