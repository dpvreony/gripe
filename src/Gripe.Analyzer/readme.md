## Introduction

This is a Roslyn analyzer package that provides compile-time diagnostics to improve software quality and consistency.

## Getting Started

You just need to install the package in your project. You can do this using the command line:

```cmd
dotnet add package Gripe.Analyzer
```

Alternatively, you can place the package in your Directory.Build.props file so it is available to all projects in the solution:

```xml
<Project>
  <ItemGroup>
	<PackageReference Include="Gripe.Analyzer" Version="$(GripeVersion)" PrivateAssets="all" />
  </ItemGroup>
</Project>
```
