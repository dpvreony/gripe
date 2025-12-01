// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gripe.DotNetTool.CommandLine;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Whipstaff.CommandLine;

namespace Gripe.DotNetTool
{
    /// <summary>
    /// Job to carry out analysis.
    /// </summary>
    public sealed class Job : AbstractCommandLineHandler<CommandLineArgModel, JobLogMessageActionsWrapper>
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="logMessageActionsWrapper">Log Message Actions Wrapper instance.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        public Job(JobLogMessageActionsWrapper logMessageActionsWrapper, IFileSystem fileSystem)
            : base(logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);

            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Carry out analysis using specified instance of MSBuild.
        /// </summary>
        /// <param name="instance">Instance of MSBuild to use.</param>
        /// <param name="solutionPath">Solution to analyze.</param>
        /// <returns>0 for success, 1 for failure.</returns>
        public async Task<int> DoAnalysis(
            VisualStudioInstance instance,
            IFileInfo solutionPath)
        {
            LogMessageActionsWrapper.UsingMsBuildAtPath(instance.MSBuildPath);

            // NOTE: Be sure to register an instance with the MSBuildLocator
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
            MSBuildLocator.RegisterInstance(instance);

            var solutionFullPath = solutionPath.FullName;
            var hasIssues = false;
            using (var workspace = MSBuildWorkspace.Create())
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.RegisterWorkspaceFailedHandler(e => LogMessageActionsWrapper.WorkspaceFailed(e));

                LogMessageActionsWrapper.StartingLoadOfSolution(solutionFullPath);

                // Attach progress reporter so we print projects as they are loaded.
                var solution = await workspace.OpenSolutionAsync(solutionFullPath, new ConsoleProgressReporter());
                LogMessageActionsWrapper.FinishedLoadOfSolution(solutionFullPath);

                var analyzers = GetDiagnosticAnalyzers();

                LogMessageActionsWrapper.StartingAnalysisOfProjects();
                var diagnosticCount = new DiagnosticCountModel();
                var groupedDiagnosticCounts = new ConcurrentDictionary<string, int>();
                foreach (var project in solution.Projects)
                {
                    hasIssues |= await AnalyzeProject(
                        project,
                        analyzers,
                        diagnosticCount,
                        groupedDiagnosticCounts)
                        .ConfigureAwait(false);
                }

                OutputDiagnosticCounts(diagnosticCount);
                OutputGroupedDiagnosticCounts(groupedDiagnosticCounts);
            }

            return hasIssues ? 1 : 0;
        }

        /// <inheritdoc/>
        protected override async Task<int> OnHandleCommand(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            // Attempt to set the version of MSBuild.
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();

            var count = visualStudioInstances.Length;

            VisualStudioInstance? instance;
            var specificMsBuildInstance = commandLineArgModel.MsBuildInstanceName;

            switch (count)
            {
                case 0:
                    LogMessageActionsWrapper.NoMsBuildInstanceFound();
                    return 1;
                case 1:
                    instance = visualStudioInstances[0];

                    if (!string.IsNullOrWhiteSpace(specificMsBuildInstance))
                    {
                        // check if instance name matches
                        if (!instance.Name.Equals(specificMsBuildInstance, StringComparison.OrdinalIgnoreCase))
                        {
                            LogMessageActionsWrapper.RequestedMsBuildInstanceNotFound(specificMsBuildInstance!);
                            return 2;
                        }
                    }

                    break;
                default:
                    instance = visualStudioInstances.OrderByDescending(x => x.Version).First();
#if TBC
                    _logMessageActionsWrapper.MultipleMsBuildInstancesFound(visualStudioInstances.Length);
                    foreach (var visualStudioInstance in visualStudioInstances)
                    {
                        _logMessageActionsWrapper.FoundMsBuildInstance(visualStudioInstance.Name, visualStudioInstance.MSBuildPath);
                    }
                    return 3;
#endif
                    break;
            }

            return await DoAnalysis(
                instance,
                commandLineArgModel.SolutionPath).ConfigureAwait(false);
        }

        private async Task<bool> AnalyzeProject(
            Project project,
            ImmutableArray<DiagnosticAnalyzer> analyzers,
            DiagnosticCountModel diagnosticCount,
            ConcurrentDictionary<string, int> groupedDiagnosticCounts)
        {
            if (project.FilePath == null)
            {
                return false;
            }

            LogMessageActionsWrapper.StartingAnalysisOfProject(project.FilePath);
            var compilation = await project.GetCompilationAsync().ConfigureAwait(false);
            if (compilation == null)
            {
                // TODO: warn about failure to get compilation object.
                LogMessageActionsWrapper.FailedToGetCompilationObjectForProject(project.FilePath);
                return true;
            }

            var compilationWithAnalyzers = compilation.WithAnalyzers(analyzers);
            var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().ConfigureAwait(false);

            OutputDiagnostics(diagnostics, diagnosticCount, groupedDiagnosticCounts);

            return !diagnostics.IsEmpty;
        }

        private ImmutableArray<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            return global::Gripe.DotNetTool.DiagnosticAnalyzerCollectionFactory.EnumerateDiagnosticAnalyzers().ToImmutableArray();
        }

        private void OutputGroupedDiagnosticCounts(ConcurrentDictionary<string, int> groupedDiagnosticCounts)
        {
            foreach (var groupedDiagnosticCount in groupedDiagnosticCounts)
            {
                LogMessageActionsWrapper.GroupedDiagnosticCount(groupedDiagnosticCount.Key, groupedDiagnosticCount.Value);
            }
        }

        private void OutputDiagnosticCounts(DiagnosticCountModel diagnosticCount)
        {
            LogMessageActionsWrapper.DiagnosticCount(diagnosticCount);
        }

        private void OutputDiagnostics(ImmutableArray<Diagnostic> diagnostics, DiagnosticCountModel diagnosticCount, ConcurrentDictionary<string, int> groupedDiagnosticCounts)
        {
            foreach (var diagnostic in diagnostics)
            {
                groupedDiagnosticCounts.AddOrUpdate(diagnostic.Id, _ => 1, (s, i) => ++i);
                OutputDiagnostic(diagnostic, diagnosticCount);
            }
        }

        private void OutputDiagnostic(Diagnostic diagnostic, DiagnosticCountModel diagnosticCount)
        {
            try
            {
                var message = diagnostic.ToString();
                switch (diagnostic.Severity)
                {
                    case DiagnosticSeverity.Error:
                        diagnosticCount.ErrorCount.Increment();
                        LogMessageActionsWrapper.DiagnosticError(message);
                        break;
                    case DiagnosticSeverity.Hidden:
                        diagnosticCount.HiddenCount.Increment();
                        LogMessageActionsWrapper.DiagnosticHidden(message);
                        break;
                    case DiagnosticSeverity.Info:
                        diagnosticCount.InformationCount.Increment();
                        LogMessageActionsWrapper.DiagnosticInfo(message);
                        break;
                    case DiagnosticSeverity.Warning:
                        diagnosticCount.WarningCount.Increment();
                        LogMessageActionsWrapper.DiagnosticWarning(message);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
        {
            public void Report(ProjectLoadProgress loadProgress)
            {
                var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                if (loadProgress.TargetFramework != null)
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})";
                }

                Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
            }
        }
    }
}
