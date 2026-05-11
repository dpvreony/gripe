## Introduction

This is a development dependency that contains a series of MSBuild tasks and Roslyn analyzers to help with managing the development of your code.

## Getting Started

You just need to install the package in your project. You can do this using the command line:

```cmd
dotnet add package Dhgms.QualityAssurancePack
```

Alternatively, you can place the package in your dir.build.props file so it is available to all projects in the solution:

```xml
<Project>
  <ItemGroup>
	<PackageReference Include="Dhgms.QualityAssurancePack" Version="$(QualityAssurancePackVersion)" PrivateAssets="all" />
  </ItemGroup>
</Project>
```
