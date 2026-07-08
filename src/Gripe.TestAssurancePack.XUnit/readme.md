## Introduction

This is a package for allowing a Testing Operating Model using xUnit V3 on MTP2.

## Getting Started

You just need to install the package in your project. You can do this using the command line:

```cmd
dotnet add package Gripe.TestAssurancePack.XUnit
```

Alternatively, you can place the package in your dir.build.props file so it is available to all projects in the solution:

```xml
<Project>
  <ItemGroup>
	<PackageReference Include="Gripe.TestAssurancePack.XUnit" Version="$(GripeVersion)" />
  </ItemGroup>
</Project>
```
