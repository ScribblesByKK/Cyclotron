# Copilot Workspace Instructions

## Repository Overview

Cyclotron is a **Windows-targeted .NET monorepo** containing reusable library packages for modern .NET applications. All projects share a common build configuration via `Directory.Build.props` and `Directory.Packages.props`.

### Projects

| Project | Purpose |
|---|---|
| `Cyclotron.Utilities` | Clean-architecture helpers and shared extension methods |
| `Cyclotron.FileSystemAdapter` | Cross-platform file-system abstraction; WinUI-specific implementations live under `WinUI/` |
| `Cyclotron.Telemetry` | Logging / telemetry adapters (Serilog-backed) |
| `Cyclotron.Tests` | All unit and integration tests for every sub-project |

---

## C# Coding Standards

These rules come from `.editorconfig` and `Directory.Build.props` and are enforced as compiler warnings/errors.

### File & Namespace Layout

- One class / interface / enum per file; file name must match the type name.
- Use **file-scoped namespaces** (`namespace Foo.Bar;`), not block-scoped.
- `using` directives go **outside** the namespace block.
- Sort `System.*` usings first, then all others alphabetically.
- Remove unused `using` directives (IDE0005 is a warning).

### Naming

| Kind | Convention | Example |
|---|---|---|
| Types (class, struct, interface, enum) | PascalCase | `FileSystemProvider` |
| Interfaces | `I` prefix + PascalCase | `IFileHandler` |
| Public / protected members (methods, properties, events) | PascalCase | `GetFileAsync` |
| Private fields | `_` prefix + camelCase | `_fileHandler` |
| `const` private fields | PascalCase (no underscore) | `DefaultBufferSize` |
| Local variables / parameters | camelCase | `fileContent` |

Do **not** prefix members with `this.`; do **not** use Hungarian notation.

### Syntax Preferences

- Prefer **`var`** over explicit types in all contexts where the type is inferrable.
- Use `new()` (target-typed new) when the type is apparent on the left side.
- Use **expression-bodied members** for:
  - Single-line property getters/setters and accessors.
  - Single-line methods (optional, `when_on_single_line`).
  - **Block bodies** are required for constructors.
- Prefer **pattern matching** over `is` casts + null checks.
- Prefer **primary constructors** (`public class Foo(IBar bar)`) when there is no constructor body.
- Use `default` instead of `default(T)`.
- Prefer `??` and `?.` over explicit null checks.
- Discard unused assignment results with `_` (`_ = SomeMethod()`).
- Use `is null` / `is not null` instead of `== null` / `!= null`.

### Braces & Spacing

- **Allman style**: every opening brace on its own line, for all code blocks including lambdas, methods, type bodies, properties, and accessors.
- Always include braces even for single-line `if`/`else`/`for`/`foreach` bodies.
- Four spaces per indent level (no tabs).
- CRLF line endings.
- Trim trailing whitespace.

### Modifier Order

```
public private protected internal static async readonly override sealed abstract virtual
```

Always declare `readonly` on fields that are never reassigned after construction.

### Accessibility Modifiers

Declare explicit accessibility modifiers on all members except public interface members.

---

## Build Defaults (`Directory.Build.props`)

Every project inherits these settings; do **not** override them locally without a documented reason:

- `<Nullable>enable</Nullable>` — nullable reference types are **always on**; treat every `#nullable disable` as a smell.
- `<ImplicitUsings>enable</ImplicitUsings>` — the standard BCL namespaces are auto-imported.
- `<LangVersion>latest</LangVersion>` — use current C# language features freely.
- `<IsAotCompatible>true</IsAotCompatible>` / `<IsTrimmable>true</IsTrimmable>` — shipping code must be AOT and trim-safe; do not call reflection APIs without a `[RequiresUnreferencedCode]` guard.
- `<EnableWindowsTargeting>true</EnableWindowsTargeting>` — the repo targets Windows; WinUI / Win32 interop is expected.
- `<GenerateDocumentationFile>true</GenerateDocumentationFile>` — all public APIs **must** have XML doc comments; CS1591 is suppressed only to allow gradual adoption, not as a permanent exemption.
- `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>` — unsafe code is allowed, but must be contained in the smallest possible scope and annotated.

---

## Change-Scope Rules

- **Fix the root cause**, not the symptom. Do not work around a bug by adding exception handling or flag-checks when the underlying code is plainly wrong.
- Keep each change focused — avoid touching files or lines that are unrelated to the task at hand.
- Preserve existing code style in files you edit, even if other parts of the file differ from these instructions.
- Do **not** reformat entire files; only adjust the lines you actually change.
- When adding a method to an existing class, match the ordering pattern already used in that class (e.g., public before private, async after sync equivalents).

---

## Public API Requirements

- Every public type and every public or protected member must have an XML doc comment (`<summary>` at minimum).
- Prefer `<inheritdoc />` on overrides / interface implementations.
- Doc comments must explain *intent and behavior*, not merely restate the member name.

---

## Dependency Injection

All libraries expose their services through a static `ServiceCollectionExtensions` class in the `DependencyInjection/` folder. The extension method must be named `AddCyclotron{Domain}(…)` and return `IServiceCollection`. Follow this pattern when adding new services.

---

## Testing Summary

All tests live in `Cyclotron.Tests/`. For detailed test conventions and patterns see the specialist skills:

- **Conventions** (stack, naming, folder layout, category attributes, scaffolding checklist): [.github/skills/cyclotron-test-specialist/test-conventions/SKILL.md](skills/cyclotron-test-specialist/test-conventions/SKILL.md)
- **Patterns** (unit blueprints, integration blueprints, mock patterns, concurrency): [.github/skills/cyclotron-test-specialist/test-patterns/SKILL.md](skills/cyclotron-test-specialist/test-patterns/SKILL.md)

Quick reference:
- Framework: **TUnit** · Assertions: **AwesomeAssertions** · Mocks: **NSubstitute**
- Every test class needs exactly **two `[Category]` attributes**: layer (`Unit` or `Integration`) + domain (sub-project suffix).
- Coverage target: ≥ 80 % line coverage for adapter classes.

Scoped instructions for test files: [.github/instructions/tests.instructions.md](instructions/tests.instructions.md)

---

## Documentation

- DocFX generates the public documentation site from XML comments and Markdown files under `docs/`.
- Scoped instructions for docs: [.github/instructions/docs.instructions.md](instructions/docs.instructions.md)
