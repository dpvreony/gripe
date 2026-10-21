## Introduction

This is a .NET tool for producing warning and error code statistics from an MSBuild binary log (`.binlog`).

## Getting Started

Install the tool using the command line:

```cmd
dotnet tool install --global Gripe.MSBuildBinLogStats.DotNetTool
```

Run the tool against a binlog file:

```cmd
gripe-msbuildlogstats --binlog-path path\to\build.binlog
```

The `--binlog-path` option is required and must point to an existing `.binlog` file.

### Example output

```text
Warnings:
  CS1591: 42
  NU1701: 3

Errors:
  CS1002: 5
```
