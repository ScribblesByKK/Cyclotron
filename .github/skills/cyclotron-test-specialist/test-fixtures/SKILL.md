---
name: cyclotron-test-fixtures
description: >
  How to design fixtures, test helpers, and custom AwesomeAssertions extensions
  in Cyclotron. Use when creating new test infrastructure for any sub-project.
---

# Cyclotron Test Fixtures & Helpers

## Fixtures

### When to Create a Fixture

Create a fixture class when **two or more integration test classes** need the same shared infrastructure — temporary directories, in-memory sinks, database contexts, HTTP test servers, cache stores, etc.

If only one test class needs the setup, inline it in `[Before(Test)]` instead.

### Where Fixtures Live

All fixture classes go in `Cyclotron.Tests/Integration/Fixtures/` regardless of which domain uses them. This keeps them discoverable and reusable across sub-projects.

### Naming Convention

```
{Purpose}Fixture.cs
```

Examples: `TempFileSystemFixture`, `TelemetryTestSink`, `InMemoryCacheFixture`, `TestHttpServer`

### Design Rules

1. **Implement `IAsyncDisposable`** (preferred) or `IDisposable`.
   ```csharp
   public class TempFileSystemFixture : IAsyncDisposable
   {
       public async ValueTask DisposeAsync()
       {
           try
           {
               if (Directory.Exists(RootPath))
                   Directory.Delete(RootPath, recursive: true);
           }
           catch
           {
               // Best-effort cleanup — swallow to prevent cascading failures
           }
           await Task.CompletedTask;
       }
   }
   ```

2. **Instantiate per test** — create in `[Before(Test)]`, dispose in `[After(Test)]`. Never share a fixture instance across tests.
   ```csharp
   [Before(Test)]
   public void Setup()
   {
       _fixture = new TempFileSystemFixture();
   }

   [After(Test)]
   public async Task Cleanup()
   {
       await _fixture.DisposeAsync();
   }
   ```

3. **GUID-based resource isolation** — every fixture instance must generate unique identifiers (GUIDs, random ports, etc.) so parallel tests never collide.
   ```csharp
   public TempFileSystemFixture()
   {
       var tempBase = Path.Combine(Path.GetTempPath(), "Cyclotron.Tests");
       RootPath = Path.Combine(tempBase, Guid.NewGuid().ToString());
       Directory.CreateDirectory(RootPath);
   }
   ```

4. **Best-effort cleanup** — `DisposeAsync` / `Dispose` must swallow exceptions. A cleanup failure should never cause a passing test to fail.

5. **Thread-safe data capture** — if the fixture collects data from concurrent operations (e.g., a log sink receiving entries from multiple threads), protect shared collections with `lock` or use `ConcurrentBag<T>`.
   ```csharp
   private readonly List<LogEntry> _logEntries = new();
   private readonly object _lock = new();

   internal void AddLog(LogLevel level, string category, string message, …)
   {
       lock (_lock)
       {
           _logEntries.Add(new LogEntry(level, category, message, …));
       }
   }
   ```

6. **Expose query helpers** — provide methods to filter or search captured data, reducing assertion boilerplate in tests.
   ```csharp
   public IEnumerable<LogEntry> GetLogs(LogLevel level)
       => LoggedEntries.Where(e => e.Level == level);

   public IEnumerable<LogEntry> GetLogs(string categoryContains)
       => LoggedEntries.Where(e => e.Category.Contains(categoryContains, StringComparison.OrdinalIgnoreCase));
   ```

### Existing Fixtures (Reference Examples)

| Fixture | Purpose | File |
|---|---|---|
| `TempFileSystemFixture` | GUID-based temp directory per test, `IAsyncDisposable` | `Cyclotron.Tests/Integration/Fixtures/TempFileSystemFixture.cs` |
| `TelemetryTestSink` | Thread-safe in-memory `ILoggerProvider` capturing `LogEntry` records | `Cyclotron.Tests/Integration/Fixtures/TelemetryTestSink.cs` |

---

## Test Helpers

### When to Create a Helper

Create a static helper class when the same setup or assertion logic appears in **two or more test methods** across different test classes.

### Where Helpers Live

All helper classes go in `Cyclotron.Tests/TestHelpers/`.

### Naming Convention

```
{Domain}TestHelpers.cs   — setup/utility methods
{Domain}Assertions.cs    — custom assertion extensions
```

### Static Helper Pattern

```csharp
namespace Cyclotron.Tests.TestHelpers;

public static class FileSystemTestHelpers
{
    public static async Task CreateTestFileAsync(string path, string content)
        => await File.WriteAllTextAsync(path, content);

    public static void CreateTestDirectory(string path)
        => Directory.CreateDirectory(path);

    public static void AssertFileExists(string path)
        => File.Exists(path).Should().BeTrue($"file should exist at {path}");

    public static async Task AssertFileContentAsync(string path, string expectedContent)
    {
        var actualContent = await File.ReadAllTextAsync(path);
        actualContent.Should().Be(expectedContent);
    }

    public static byte[] GenerateRandomBytes(int count)
    {
        var bytes = new byte[count];
        Random.Shared.NextBytes(bytes);
        return bytes;
    }

    public static async Task CreateLargeFileAsync(string path, int sizeInMB)
    {
        var bytes = GenerateRandomBytes(sizeInMB * 1024 * 1024);
        await File.WriteAllBytesAsync(path, bytes);
    }
}
```

- Wrap repetitive setup (file creation, directory creation, data generation).
- Use `Should()` from AwesomeAssertions for assertion helpers, with a descriptive `because` clause.

### Existing Helpers (Reference Examples)

| Helper | File |
|---|---|
| `FileSystemTestHelpers` | `Cyclotron.Tests/TestHelpers/FileSystemTestHelpers.cs` |
| `TelemetryAssertions` | `Cyclotron.Tests/TestHelpers/TelemetryAssertions.cs` |

---

## Custom Assertion Extensions

### When to Create

Create custom assertion extensions when you find yourself writing the same multi-step `Should().Contain(e => …)` assertion in multiple test methods. They make tests more readable and produce better failure messages.

### Pattern

Write them as **extension methods on the fixture type** (or on the subject type), using AwesomeAssertions internally:

```csharp
using AwesomeAssertions;
using Cyclotron.Tests.Integration.Fixtures;
using Microsoft.Extensions.Logging;

namespace Cyclotron.Tests.TestHelpers;

public static class TelemetryAssertions
{
    public static void ShouldHaveLogged(this TelemetryTestSink sink, LogLevel level, string messageContains)
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.Level == level && e.Message.Contains(messageContains, StringComparison.OrdinalIgnoreCase),
                $"expected log with level {level} and message containing '{messageContains}'");
    }

    public static void ShouldHaveLoggedException<TException>(this TelemetryTestSink sink)
        where TException : Exception
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.Exception is TException,
                $"expected log with exception of type {typeof(TException).Name}");
    }

    public static void ShouldHaveScope(this TelemetryTestSink sink, string key, object value)
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.State.ContainsKey(key) && Equals(e.State[key], value),
                $"expected log with scope property {key}={value}");
    }
}
```

### Rules for Custom Assertions

- **Always include a `because` clause** — explains what the assertion expected in failure output.
- **Method naming**: `.ShouldHave*()`, `.ShouldBe*()`, `.ShouldContain*()`.
- **One concern per method** — keep each assertion extension focused on a single check.
- **Place in `TestHelpers/{Domain}Assertions.cs`** — colocated with the domain's other helpers.
