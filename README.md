# Cyclotron

[![Tests](https://github.com/ScribblesByKK/Cyclotron/actions/workflows/test.yml/badge.svg)](https://github.com/ScribblesByKK/Cyclotron/actions/workflows/test.yml)
[![Code Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/Kumara-Krishnan/3884cb12468c35c41f70e4222b65c727/raw/cyclotron-coverage.json)](https://github.com/ScribblesByKK/Cyclotron/actions/workflows/test.yml)

A comprehensive .NET library collection for modern .NET applications.

## Projects

### Cyclotron.Utilities
Core utilities and clean architecture components for .NET applications.

### Cyclotron.FileSystemAdapter
Cross-platform file system abstraction with WinUI support for Windows applications.

### Cyclotron.Telemetry
Logging and telemetry adapters for .NET applications.

## Testing

This project maintains high code quality with comprehensive unit and integration tests:

- **Unit Tests**: Fast, isolated tests with mocked dependencies
- **Integration Tests**: Real infrastructure tests (file system, logging providers)
- **Coverage Target**: >80% line coverage for adapter classes

Run tests locally:

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "Category=Unit"

# Run only integration tests
dotnet test --filter "Category=Integration"

# Run with coverage
dotnet test -- --coverage --coverage-output-format cobertura
```

## Documentation

Full documentation is available at [https://kumara-krishnan.github.io/Cyclotron/](https://kumara-krishnan.github.io/Cyclotron/)

To build documentation locally:

```bash
dotnet tool install -g docfx
cd docs
docfx docfx.json --serve
```

See [docs/README.md](docs/README.md) for more information about the documentation system.

## Copilot Instructions

Workspace-level Copilot instructions live in `.github/` and are automatically picked up by GitHub Copilot in VS Code:

| File | Scope |
|---|---|
| [`.github/copilot-instructions.md`](.github/copilot-instructions.md) | All files — C# style, build defaults, change-scope rules, DI conventions |
| [`.github/instructions/tests.instructions.md`](.github/instructions/tests.instructions.md) | `Cyclotron.Tests/**` — TUnit/AwesomeAssertions/NSubstitute patterns, category attributes |
| [`.github/instructions/docs.instructions.md`](.github/instructions/docs.instructions.md) | `docs/**` — DocFX Markdown style, toc.yml rules, XML doc comment quality |

Detailed test specialist guidance lives in [`.github/skills/cyclotron-test-specialist/`](.github/skills/cyclotron-test-specialist/).

## Contributing

Contributions are welcome! Please ensure:
- All public APIs are documented with XML comments
- New features include unit and integration tests
- Code coverage remains above 80%

## License

See LICENSE file for details.