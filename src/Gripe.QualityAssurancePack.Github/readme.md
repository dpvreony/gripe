## Introduction

This is a development dependency that contains a series of settings and packages for building and maintaining .NET projects on GitHub.

## Getting Started

You just need to install the package in your project. You can do this using the command line:

```cmd
dotnet add package Dhgms.QualityAssurancePack.Github
```

Alternatively, you can place the package in your dir.build.props file so it is available to all projects in the solution:

```xml
<Project>
  <ItemGroup>
	<PackageReference Include="Dhgms.QualityAssurancePack.Github" Version="$(QualityAssurancePackVersion)" PrivateAssets="all" />
  </ItemGroup>
</Project>
```
