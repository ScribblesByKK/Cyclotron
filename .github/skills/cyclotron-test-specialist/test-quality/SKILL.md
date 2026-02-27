---
name: cyclotron-test-quality
description: >
  Flakiness prevention, acceptance criteria, success criteria, and CI coverage
  requirements for Cyclotron tests. Use when reviewing test quality or evaluating
  completeness of a test suite.
---

# Cyclotron Test Quality

## Flakiness Prevention

Every rule here exists because the opposite caused real flaky tests. Treat violations as bugs.

### 1. GUID-Based Resource Isolation

Every test must use **unique** resource identifiers — file paths, directory names, keys, ports. Use `Guid.NewGuid()` or fixture helpers that generate them. Never hardcode shared paths like `C:\temp\test.txt`.

```csharp
// GOOD
var filePath = _fixture.GetTempFilePath($"parallel_{i}.txt");

// BAD — another test may use the same file
var filePath = Path.Combine(Path.GetTempPath(), "test.txt");
```

### 2. Best-Effort Cleanup

`DisposeAsync` / `Dispose` in fixtures **must swallow exceptions**. A file locked by a virus scanner or a directory already deleted by the OS should not turn a green test red.

```csharp
public async ValueTask DisposeAsync()
{
    try { Directory.Delete(RootPath, recursive: true); }
    catch { /* best effort */ }
    await Task.CompletedTask;
}
```

### 3. Thread-Safe Shared Infrastructure

Any fixture that captures concurrent data (`TelemetryTestSink`, future event collectors, etc.) must protect shared state:

```csharp
private readonly object _lock = new();

internal void AddEntry(Entry entry)
{
    lock (_lock) { _entries.Add(entry); }
}
```

Or use `ConcurrentBag<T>` / `ConcurrentQueue<T>`. Never use a bare `List<T>` for concurrent writes.

### 4. Fresh State Per Test

- Create mocks in `[Before(Test)]`, not as field initialisers or in constructors.
- Create fixture instances in `[Before(Test)]`, dispose in `[After(Test)]`.
- **Never share mutable state** between tests — no `static` fields, no class-level singletons that carry state across tests.

### 5. No Timing Dependencies

- Never use `Task.Delay()` or `Thread.Sleep()` to wait for an async operation to "settle."
- If you need synchronisation, use proper primitives: `SemaphoreSlim`, `TaskCompletionSource`, `ManualResetEventSlim`.
- If a component fundamentally requires a timer (e.g., a debounce), inject `TimeProvider` and use a fake in tests.

### 6. Bounded Concurrency

- Cap `Task.WhenAll` parallel tests at **20–50 tasks** — enough to surface race conditions, not enough to thrash CI runners.
- Each parallel task must target a **unique resource** (different file, different key, different logger category).

### 7. Unique Filenames

Never assume a prior test cleaned up its files. Always generate unique names:

```csharp
var filePath = _fixture.GetTempFilePath(); // auto-generates GUID.tmp
```

---

## Acceptance Criteria (Per Sub-Project)

When writing or reviewing tests for a sub-project, verify all of the following are covered:

### Happy Path Coverage

- [ ] Every **public method** has at least **one unit test** exercising its primary success scenario.
- [ ] Every method with return values has a test that asserts the returned value.

### Error & Exception Coverage

- [ ] Every public method that can throw or propagate exceptions has at least **one error test**.
- [ ] Exception type and message are both asserted where meaningful:
  ```csharp
  await act.Should().ThrowAsync<FileNotFoundException>()
      .WithMessage("File not found");
  ```

### Integration Coverage

- [ ] End-to-end flows are tested with real implementations (not mocks):
  - Write → Read round-trips
  - Register → Resolve → Use DI pipelines
  - Produce → Capture → Assert observable side effects
- [ ] DI service registration is verified for every critical service:
  ```csharp
  _serviceProvider.GetService<ICyclotronLogger>().Should().NotBeNull();
  ```

### Concurrency Coverage

- [ ] Any class or method documented or designed as thread-safe has a parallel test (20–50 tasks).
- [ ] Concurrent writes to independent resources all succeed.
- [ ] Concurrent access to shared resources does not throw.

### Configuration Coverage

- [ ] Every options / configuration class has a **defaults test** pinning each property's initial value.
- [ ] Custom configuration values are tested by overriding defaults in `[Before(Test)]`.

### Enum Coverage

- [ ] Every enum type has a test verifying:
  - Total member count (`.Should().HaveCount(n)`)
  - Ordinal positions of each member (catches reordering / insertions)

---

## Success Criteria

A test suite is considered **complete** when:

| Criterion | Target |
|---|---|
| Line coverage per sub-project | **≥ 80%** |
| Unit test category present | `[Category("Unit")]` on every unit test class |
| Integration test category present | `[Category("Integration")]` on every integration test class |
| Domain category present | `[Category("{Domain}")]` matching sub-project on every test class |
| Independent filter runs | `dotnet test --filter "Category=Unit"` passes independently |
| | `dotnet test --filter "Category=Integration"` passes independently |
| No flakiness flags | None of the anti-patterns from the Flakiness Prevention section remain |

---

## AwesomeAssertions Idiom Reference

Quick reference for the assertion patterns used across the codebase. All require `using AwesomeAssertions;`.

### Equality

```csharp
result.Should().Be(expected);               // value equality
result.Should().BeSameAs(expected);          // reference equality
result.Should().NotBeSameAs(other);
```

### Collections

```csharp
list.Should().HaveCount(3);
list.Should().Contain(item);
list.Should().Contain(e => e.Name == "x");   // predicate
list.Should().ContainSingle();
list.Should().BeEmpty();
list.Should().AllBe(expected);
list.Should().BeEquivalentTo(expected);
list.Should().HaveCountGreaterThanOrEqualTo(50);
```

### Booleans

```csharp
value.Should().BeTrue();
value.Should().BeFalse();
```

### Nulls

```csharp
obj.Should().BeNull();
obj.Should().NotBeNull();
str.Should().NotBeNullOrEmpty();
```

### Exceptions (Synchronous)

```csharp
var act = () => SomeMethod();
act.Should().Throw<InvalidOperationException>();
act.Should().NotThrow();
```

### Exceptions (Asynchronous)

```csharp
var act = async () => await SomeMethodAsync();
await act.Should().ThrowAsync<FileNotFoundException>()
    .WithMessage("File not found");
await act.Should().NotThrowAsync();
```

### Custom Because Clauses

Always add a `because` clause when the assertion's intent may not be obvious:

```csharp
File.Exists(path).Should().BeTrue($"file should exist at {path}");
```

---

## CI Integration

Tests run in CI via `.github/workflows/test.yml`:

- **Trigger**: Push to `main` / `ci`, PRs targeting `main` / `ci`, manual dispatch.
- **Coverage tool**: Coverlet (both `Coverlet.Collector` and `Coverlet.MSBuild`).
- **Coverage command**:
  ```bash
  dotnet test Cyclotron.Tests/Cyclotron.Tests.csproj \
    --configuration Release \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=./coverage/coverage.cobertura.xml
  ```
- **Thresholds**: Warn at **60%**, target **80%** (badge turns green at ≥ 80%).
- **PR comments**: Coverage summary posted automatically via `CodeCoverageSummary`.
- **Badge**: Auto-published to a GitHub Gist, displayed in `README.md`.
