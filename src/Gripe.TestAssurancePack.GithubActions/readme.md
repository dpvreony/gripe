## Introduction

This is a package for allowing a Testing Operating Model when working in Github Actions.

## Getting Started

You just need to install the package in your project. You can do this using the command line:

```cmd
dotnet add package Gripe.TestAssurancePack.GithubActions
```

Alternatively, you can place the package in your dir.build.props file so it is available to all projects in the solution:

```xml
<Project>
  <ItemGroup>
	<PackageReference Include="Gripe.TestAssurancePack.GithubActions" Version="$(GripeVersion)" />
  </ItemGroup>
</Project>
```
