# Introduction to Cyclotron

Cyclotron is a modern .NET library collection that provides essential utilities and adapters for building robust applications.

## Overview

Cyclotron consists of several libraries, each designed to solve specific problems in .NET development:

### Cyclotron.Utilities

The utilities library provides:
- Clean architecture components
- Common patterns and abstractions
- Helper methods for .NET applications

### Cyclotron.FileSystemAdapter

A cross-platform file system abstraction that provides:
- Unified file system operations
- Support for both .NET 10.0 and Windows-specific implementations
- WinUI integration for Windows applications
- Dependency injection support

## Installation

Install the libraries via NuGet:

```bash
dotnet add package Cyclotron.Utilities
dotnet add package Cyclotron.FileSystemAdapter
```

## Quick Start

### Using Cyclotron.FileSystemAdapter

```csharp
using Cyclotron.FileSystemAdapter;
using Microsoft.Extensions.DependencyInjection;

// Register services
services.AddFileSystemServices();

// Use the file system provider
var provider = serviceProvider.GetRequiredService<IFileSystemProvider>();
```

## Next Steps

- Explore the [API Documentation](../api/index.md)
- Check out more [articles and guides](toc.yml)
