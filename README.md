# Gripe

Gripe is a collection of developer-facing components for applying code quality checks across .NET solutions. The repository currently exposes six main consumption points:

## Mission statement

Gripe exists to improve the developer experience in .NET by making quality checks easier to adopt, easier to run, and easier to understand. Its packages and tools are intended to reduce software development risk by surfacing maintainability, build, packaging, and testing issues earlier in the development lifecycle.

1. Roslyn analyzers for compile-time diagnostics.
2. A dotnet tool for running the analyzers without installing the analyzer package into the target solution.
3. A dotnet tool for summarizing warning and error counts from an MSBuild binlog.
4. MSBuild rules and tasks for enforcing build-time quality conventions.
5. A Quality Assurance Pack that bundles analyzers and MSBuild checks into a single package.
6. A TestAssurancePack for xUnit on Microsoft Testing Platform v2.

## Components

| Component | Package / Command | Purpose |
| --- | --- | --- |
| Roslyn analyzers | `Gripe.Analyzer` | Adds analyzer diagnostics directly to projects during build and in the IDE. |
| Analyzer dotnet tool | `gripe-scanner` | Runs Gripe analysis against a solution file without adding the analyzer package to that solution. |
| MSBuild binlog stats tool | `gripe-msbuildlogstats` | Reads an MSBuild `.binlog` and prints grouped warning and error counts by diagnostic code. |
| MSBuild rules and tasks | `Gripe.MsBuild` | Imports build targets and custom tasks that enforce repository and packaging conventions. |
| Quality Assurance Pack | `Gripe.QualityAssurancePack` | Central package for applying Gripe analyzers plus MSBuild quality checks to a codebase. |
| TestAssurancePack | `Gripe.TestAssurancePack.XUnit` | Central package for unit testing with xUnit using Microsoft Testing Platform v2. |

## 1. Roslyn analyzers

Use the analyzer package when you want diagnostics to run as part of normal development in the IDE and during project builds.

```bash
dotnet add package Gripe.Analyzer
```

What it provides:

- Custom Gripe analyzer diagnostics packaged under `analyzers/dotnet/cs`.
- Standard analyzer consumption through NuGet, so projects get feedback during compilation.
- A focused entry point when you only want the analyzers and not the broader build-policy package.

Use this when the target codebase can accept a development dependency and you want diagnostics to travel with the project.

## 2. Analyzer dotnet tool

Use the analyzer tool when you want to scan a solution without modifying that solution's package references.

Install the tool:

```bash
dotnet tool install --global gripe-scanner
```

Run it against a solution:

```bash
gripe-scanner path\to\YourSolution.sln
```

Optional argument:

- `--msbuild-instance-name` lets you select a specific MSBuild instance when needed.

What it does:

- Loads the solution through `MSBuildWorkspace`.
- Runs the Gripe analyzers across each project.
- Emits diagnostics plus grouped counts by diagnostic ID.
- Returns a non-zero exit code when issues are found.

Use this when you want analyzer results in CI, validation scripts, or one-off scans without installing `Gripe.Analyzer` into the scanned solution.

## 3. MSBuild binlog stats dotnet tool

Use the binlog stats tool when you already have an MSBuild structured log and want a quick breakdown of warning and error counts.

Install the tool:

```bash
dotnet tool install --global gripe-msbuildlogstats
```

Run it against a binlog:

```bash
gripe-msbuildlogstats --binlog-path path\to\build.binlog
```

What it does:

- Parses the `.binlog` file.
- Walks the build tree and collects warning codes.
- Walks the build tree and collects error codes.
- Prints a descending count summary for both warnings and errors.

Use this when the question is not "what failed" but "what kinds of failures dominate this build?"

## 4. MSBuild rules and tasks

`Gripe.MsBuild` is the lower-level build-policy package. It imports the following targets.

| Code | Target / Rule | Summary |
| --- | --- | --- |
| `QA0001` | Check analyzer references | Errors when a `ProjectReference` marked as an analyzer does not set `ReferenceOutputAssembly="false"`. |
| `QA0002` | Check project naming | Errors when a project name does not start with the solution name. |
| `QA0003` | Check project folder layout | Errors when the project file path does not match the expected `SolutionDir\ProjectName\ProjectFileName` layout. |
| `QA0004` | Check project references for executables | Warns when a project references another project that builds to `Exe`. |
| `QA0005` | Check dotnet tool project suffix | Errors when a project packed as a dotnet tool does not end with `.DotNetTool`. |
| `QA0006` | Check obsolete `netcoreapp` target frameworks | Warns when `netcoreapp5.0+` is used instead of `net5.0+`. |
| `QA0007` | Check package vs framework references | Warns when a `PackageReference` appears to duplicate a framework-provided reference assembly. |
| `QA0008` | Check debug symbol format | Warns when `DebugType` is not `portable`. |
| `QA0009` | Require package readme | Errors during packing when a package does not include exactly one root-level `readme.md`. |
| `QA0010` | Warn on Unicode package identifiers | Runs a custom task to flag package references that contain Unicode identifiers. |
| `QA0011` | Check dotnet tool assembly prefix | Errors when `PackAsTool=true` and `AssemblyName` does not start with `dotnettool-`. |
| `QA0012` | NuGet package replacement rules | Runs a custom task that can apply replacement guidance from `Gripe.MsBuild.NugetReplacementPackages.json`. |

Install it directly when you want the build rules without the broader aggregate package:

```bash
dotnet add package Gripe.MsBuild
```

Use this when you want explicit control over the build-policy layer and are choosing packages individually.

## 5. Quality Assurance Pack

`Gripe.QualityAssurancePack` is the main "apply quality guardrails to a codebase" package. It bundles:

- `Gripe.Analyzer`
- `Gripe.MsBuild`
- Several third-party analyzer packages used as development dependencies

Install it in a single project:

```bash
dotnet add package Gripe.QualityAssurancePack
```

Or apply it centrally from `Directory.Build.props`:

```xml
<Project>
	<ItemGroup>
		<PackageReference Include="Gripe.QualityAssurancePack" Version="$(GripeVersion)" PrivateAssets="all" />
	</ItemGroup>
</Project>
```

Use this when you want one package to turn on analyzer coverage and build-time quality checks across a solution.

## 6. TestAssurancePack

`Gripe.TestAssurancePack.XUnit` is the test-focused companion package for unit test projects using xUnit on Microsoft Testing Platform v2.

It brings in a curated test stack including:

- `xunit.v3.mtp-v2`
- `Microsoft.Testing.Platform`
- `Microsoft.NET.Test.Sdk`
- `Microsoft.Testing.Extensions.CodeCoverage`
- `xunit.analyzers`
- Supporting test utilities used by this repository

Install it in a test project:

```bash
dotnet add package Gripe.TestAssurancePack.XUnit
```

Or reference it centrally:

```xml
<Project>
	<ItemGroup>
		<PackageReference Include="Gripe.TestAssurancePack.XUnit" Version="$(GripeVersion)" />
	</ItemGroup>
</Project>
```

The included targets enable Microsoft Testing Platform support for test projects, including the repository defaults for showing test failures and using the platform runner.

Use this when you want a standardized unit-testing package rather than assembling the xUnit + MTP2 stack project by project.

## Choosing the right component

Use `Gripe.Analyzer` if you only need diagnostics.

Use `gripe-scanner` if you need diagnostics without package installation.

Use `gripe-msbuildlogstats` if you need counts from a `.binlog`.

Use `Gripe.MsBuild` if you need the build-policy targets directly.

Use `Gripe.QualityAssurancePack` if you want the main all-in-one quality package.

Use `Gripe.TestAssurancePack.XUnit` if you want a curated xUnit-on-MTP2 test package.

## Contributing

1. Fork the repository.
2. Make the required changes.
3. Submit a pull request.
