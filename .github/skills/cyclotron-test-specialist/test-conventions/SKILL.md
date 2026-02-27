---
name: cyclotron-test-conventions
description: >
  Cyclotron testing stack, naming rules, folder structure, category attributes,
  and new sub-project scaffolding checklist. Use when writing, scaffolding, or
  reviewing any test in Cyclotron.
---

# Cyclotron Test Conventions

## Mandatory Stack

All test code in the Cyclotron monorepo **must** use the following libraries. Versions are centrally managed in `Directory.Packages.props`.

| Library | Purpose | Namespace / Import |
|---|---|---|
| **TUnit** | Test framework | Attributes: `[Test]`, `[Before(Test)]`, `[After(Test)]`, `[Category("…")]` |
| **AwesomeAssertions** | Assertions | `using AwesomeAssertions;` — Apache 2.0 community fork |
| **NSubstitute** | Mocking | `using NSubstitute;` — `Substitute.For<T>()`, `.Returns()`, `.Received()` |

**Do not** use xUnit, NUnit, MSTest, FluentAssertions, Moq, or raw `Assert.*` anywhere in `Cyclotron.Tests/`.

## Test Naming Convention

```
MethodUnderTest_Scenario_ExpectedBehavior
```

Examples:
- `WriteAllTextAsync_ThenReadAllTextAsync_ReturnsSameContent`
- `CopyAsync_WithFailIfExists_CallsHandlerCorrectly`
- `GetFileFromPathAsync_PropagatesException_WhenHandlerThrows`
- `Constructor_WithValidParameters_Succeeds`
- `LogDebug_WithCallerInfoDisabled_LogsWithoutEnrichment`

## Folder Structure

All tests live in the `Cyclotron.Tests` project. Mirror the sub-project being tested using unit and integration folders:

```
Cyclotron.Tests/
  Unit/
    {Domain}/                    ← one file per source class
      {Domain}Tests.cs
  Integration/
    {Domain}/
      {Domain}IntegrationTests.cs
    Fixtures/                    ← shared fixtures (all domains)
      {Purpose}Fixture.cs
  TestHelpers/                   ← shared static helpers & assertion extensions
    {Domain}TestHelpers.cs
    {Domain}Assertions.cs
```

`{Domain}` matches the sub-project suffix: `FileSystemAdapter`, `Telemetry`, `Utilities`, `Caching`, `Http`, etc.

## Category Attributes

Every test class **must** have exactly two `[Category]` attributes:

1. **Layer**: `[Category("Unit")]` or `[Category("Integration")]`
2. **Domain**: `[Category("{Domain}")]` — matches the sub-project suffix

```csharp
[Category("Unit")]
[Category("FileSystemAdapter")]
public class FileSystemAdapterTests { … }

[Category("Integration")]
[Category("Telemetry")]
public class TelemetryAdapterIntegrationTests { … }
```

This enables filtered test runs:
```bash
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
dotnet test --filter "Category=FileSystemAdapter"
```

## Region Grouping

Group related tests inside `#region` blocks by logical concern:

```csharp
#region Copy Operations
// tests for copy-related methods
#endregion

#region Error Propagation
// tests that verify exception forwarding
#endregion

#region Concurrency
// parallel / thread-safety tests
#endregion
```

Choose region names that describe the **operation or behavior**, not the test method name.

## New Sub-Project Scaffolding Checklist

When adding tests for a brand-new sub-project (e.g., `Cyclotron.Caching`):

1. **Add project reference** to `Cyclotron.Tests/Cyclotron.Tests.csproj`:
   ```xml
   <ProjectReference Include="..\Cyclotron.Caching\Cyclotron.Caching.csproj" />
   ```
2. **Create unit test folder and class**: `Unit/Caching/CachingTests.cs`
3. **Create integration test folder and class**: `Integration/Caching/CachingIntegrationTests.cs`
4. **Create fixture** in `Integration/Fixtures/` if integration tests need shared infrastructure (see the `test-fixtures` skill)
5. **Create helpers** in `TestHelpers/CachingTestHelpers.cs` if setup/assertion logic will be reused
6. **Optionally create custom assertions** in `TestHelpers/CachingAssertions.cs`
7. **Apply dual `[Category]` attributes**: `[Category("Unit")]`/`[Category("Integration")]` + `[Category("Caching")]`
8. **Write tests** following unit and integration blueprints (see the `test-patterns` skill)
9. **Verify coverage** ≥ 80% for the new sub-project
