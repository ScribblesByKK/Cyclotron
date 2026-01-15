# Cyclotron Documentation

This directory contains the DocFX documentation setup for the Cyclotron mono repository.

## Structure

```
docs/
├── docfx.json          # DocFX configuration file
├── index.md            # Documentation homepage
├── toc.yml             # Main table of contents
├── articles/           # Articles and guides
│   ├── intro.md        # Introduction article
│   └── toc.yml         # Articles table of contents
└── api/                # API documentation (auto-generated)
    └── index.md        # API documentation landing page
```

## Building Documentation Locally

### Prerequisites

- .NET 10.0 SDK or later
- DocFX tool

### Install DocFX

```bash
dotnet tool install -g docfx
```

### Build the Documentation

```bash
cd docs
docfx docfx.json
```

The generated documentation will be in the `_site` folder.

### Serve Documentation Locally

```bash
cd docs
docfx docfx.json --serve
```

This will start a local web server at `http://localhost:8080`.

## GitHub Pages Deployment

The documentation is automatically built and deployed to GitHub Pages when changes are pushed to the `main` branch.

The deployment is handled by the `.github/workflows/docfx.yml` GitHub Actions workflow.

### Viewing the Documentation

Once deployed, the documentation will be available at:
https://kumara-krishnan.github.io/Cyclotron/

## Project Configuration

Documentation generation is enabled for all projects via `Directory.Build.Props`:

```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);CS1591</NoWarn>
    <DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(AssemblyName).xml</DocumentationFile>
</PropertyGroup>
```

## Adding Documentation

### XML Comments

Add XML documentation comments to your code:

```csharp
/// <summary>
/// Description of the class or method
/// </summary>
/// <param name="paramName">Parameter description</param>
/// <returns>Return value description</returns>
public class MyClass
{
    // Implementation
}
```

### Articles

Add new articles by creating Markdown files in the `articles/` directory and updating `articles/toc.yml`.

## Multi-Platform Support

The documentation build is configured to target `net10.0` for cross-platform compatibility. Projects with Windows-specific targets may show build warnings but documentation generation will continue for the supported platforms.
