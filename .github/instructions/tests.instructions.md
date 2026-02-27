---
applyTo: "Cyclotron.Tests/**"
---

# Test File Instructions

These rules apply to every file under `Cyclotron.Tests/`. They supplement the workspace-wide rules in `.github/copilot-instructions.md`. When in conflict, these rules take precedence for test code.

For the canonical, authoritative test guidance see:
- **Stack, naming, folder layout, categories, scaffolding checklist**: [.github/skills/cyclotron-test-specialist/test-conventions/SKILL.md](../skills/cyclotron-test-specialist/test-conventions/SKILL.md)
- **Unit/integration blueprints, mock patterns, concurrency**: [.github/skills/cyclotron-test-specialist/test-patterns/SKILL.md](../skills/cyclotron-test-specialist/test-patterns/SKILL.md)

---

## Mandatory Stack

| Library | Purpose |
|---|---|
| **TUnit** | Test framework — use `[Test]`, `[Before(Test)]`, `[After(Test)]`, `[Category("…")]` |
| **AwesomeAssertions** | Assertions — `using AwesomeAssertions;` |
| **NSubstitute** | Mocking — `Substitute.For<T>()`, `.Returns()`, `.Received()` |

**Never** use xUnit, NUnit, MSTest, FluentAssertions, Moq, or raw `Assert.*`.

---

## Test Naming

```
MethodUnderTest_Scenario_ExpectedBehavior
```

Examples: `WriteAllTextAsync_ThenReadAllTextAsync_ReturnsSameContent`, `CopyAsync_WithFailIfExists_CallsHandlerCorrectly`

---

## Folder & File Layout

```
Cyclotron.Tests/
  Unit/{Domain}/{Domain}Tests.cs
  Integration/{Domain}/{Domain}IntegrationTests.cs
  Integration/Fixtures/{Purpose}Fixture.cs
  TestHelpers/{Domain}TestHelpers.cs
```

`{Domain}` matches the sub-project suffix: `FileSystemAdapter`, `Telemetry`, `Utilities`, etc.

---

## Category Attributes

Every test class must have **exactly two** `[Category]` attributes — one for layer, one for domain:

```csharp
[Category("Unit")]          // or "Integration"
[Category("FileSystemAdapter")]  // sub-project suffix
public class FileSystemAdapterTests { … }
```

---

## Key Patterns

### Unit tests — mock in `[Before(Test)]`, use `null!` fields

```csharp
private ISomeDependency _dep = null!;

[Before(Test)]
public void Setup()
{
    _dep = Substitute.For<ISomeDependency>();
}
```

No `[After(Test)]` needed for mocks; no resources to dispose.

### Integration tests — DI container per test, always dispose

```csharp
private ServiceProvider _serviceProvider = null!;

[Before(Test)]
public void Setup()
{
    var services = new ServiceCollection();
    services.AddCyclotron{Domain}(/* test config */);
    _serviceProvider = services.BuildServiceProvider();
}

[After(Test)]
public async Task Cleanup()
{
    _serviceProvider.Dispose();
    await _fixture.DisposeAsync();
}
```

Never share a `ServiceProvider` between tests.

### Assertion style

- `.Should().BeSameAs()` — reference equality (mock instance)
- `.Should().Be()` — value equality
- `await act.Should().ThrowAsync<TException>()` — for async exception assertions
- Assign the `act` lambda before asserting: `var act = async () => await …;`

### Exception propagation

```csharp
_dep.SomeMethod(Arg.Any<string>())
    .Returns<T>(x => throw new InvalidOperationException("…"));
```

### Verify call counts

- `_dep.Received(1).Method(…)` — expected exactly once
- `_dep.DidNotReceive().Method(…)` — must not be called

---

## Coverage Target

≥ 80 % line coverage for all adapter classes in `Cyclotron.FileSystemAdapter` and `Cyclotron.Telemetry`.

---

## New Sub-Project Scaffolding Checklist

1. Add `<ProjectReference>` to `Cyclotron.Tests.csproj`
2. Create `Unit/{Domain}/{Domain}Tests.cs`
3. Create `Integration/{Domain}/{Domain}IntegrationTests.cs`
4. Create fixture in `Integration/Fixtures/` if needed
5. Add helpers to `TestHelpers/{Domain}TestHelpers.cs`
6. Apply dual `[Category]` attributes on every test class
7. Verify coverage ≥ 80%
