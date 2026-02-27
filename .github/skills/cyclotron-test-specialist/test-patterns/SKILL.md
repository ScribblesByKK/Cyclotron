---
name: cyclotron-test-patterns
description: >
  Unit and integration test blueprints for Cyclotron sub-projects. Use when
  writing test methods, setting up mocks, configuring DI containers, or writing
  concurrency tests.
---

# Cyclotron Test Patterns

## Unit Test Blueprint

Unit tests verify a single class or interface contract in complete isolation. Every dependency is mocked.

### Setup

```csharp
using AwesomeAssertions;
using NSubstitute;

[Category("Unit")]
[Category("{Domain}")]
public class {Domain}Tests
{
    private ISomeDependency _dep = null!;
    private ISomeOtherDep _otherDep = null!;

    [Before(Test)]
    public void Setup()
    {
        _dep = Substitute.For<ISomeDependency>();
        _otherDep = Substitute.For<ISomeOtherDep>();
    }
}
```

- Create mocks in `[Before(Test)]` — each test gets a fresh set.
- Use `null!` for mock fields to satisfy nullability without constructors.
- No `[After(Test)]` needed — mocks have no resources to dispose.

### Pattern: Verify Method Delegation

Confirm that a method call is forwarded to the underlying handler with the correct arguments.

```csharp
[Test]
public async Task CopyAndReplaceAsync_CallsHandler_WithCorrectParameters()
{
    var destinationFile = Substitute.For<IFile>();

    await _fileHandler.CopyAndReplaceAsync(_mockFile, destinationFile);

    await _fileHandler.Received(1).CopyAndReplaceAsync(_mockFile, destinationFile);
}
```

Use `Received(1)` for expected calls, `DidNotReceive()` for calls that should not happen.

### Pattern: Verify Return Values

```csharp
[Test]
public async Task GetFileFromPathAsync_ReturnsFile_WhenPathIsValid()
{
    const string path = "/test/file.txt";
    _fileHandler.GetFileFromPathAsync(path).Returns(_mockFile);

    var result = await _fileHandler.GetFileFromPathAsync(path);

    result.Should().BeSameAs(_mockFile);
}
```

- `.Should().BeSameAs()` for reference equality (same mock instance).
- `.Should().Be()` for value equality.

### Pattern: Verify Exception Propagation

```csharp
[Test]
public async Task GetFileFromPathAsync_PropagatesException_WhenHandlerThrows()
{
    _fileHandler.GetFileFromPathAsync(Arg.Any<string>())
        .Returns<IFile>(x => throw new FileNotFoundException("File not found"));

    var act = async () => await _fileHandler.GetFileFromPathAsync("nonexistent.txt");

    await act.Should().ThrowAsync<FileNotFoundException>()
        .WithMessage("File not found");
}
```

- Use `Arg.Any<T>()` for wildcard argument matching.
- Use `.Returns<T>(x => throw …)` to configure throwing mocks.
- Assign the action to `var act` before asserting— this is the standard pattern.

### Pattern: Verify Configuration Defaults

```csharp
[Test]
public void CyclotronTelemetryOptions_HasCorrectDefaults()
{
    var options = new CyclotronTelemetryOptions();

    options.ServiceName.Should().Be("CyclotronApp");
    options.ServiceVersion.Should().Be("1.0.0");
    options.Environment.Should().Be("development");
    options.DefaultModule.Should().Be("core");
    options.Logging.Should().NotBeNull();
}
```

Every options/configuration class should have a defaults test that pins each property's initial value.

### Pattern: Verify Enum Values

```csharp
[Test]
public void NameCollisionOption_HasExpectedValues()
{
    Enum.GetValues<NameCollisionOption>().Should().HaveCount(3);
    NameCollisionOption.GenerateUniqueName.Should().Be((NameCollisionOption)0);
    NameCollisionOption.ReplaceExisting.Should().Be((NameCollisionOption)1);
    NameCollisionOption.FailIfExists.Should().Be((NameCollisionOption)2);
}
```

Check both the count and ordinal positions — this catches accidental reordering or additions.

### Pattern: NSubstitute Fluent Chain

When the class under test calls a fluent/builder method like `.ForContext(…).Write(…)`, configure the mock to return itself:

```csharp
_serilogLogger.ForContext(Arg.Any<ILogEventEnricher>()).Returns(_serilogLogger);
```

This ensures subsequent calls on the chain are trackable via `Received()`.

---

## Integration Test Blueprint

Integration tests exercise real implementations, real DI containers, and real I/O. Only external systems (databases, HTTP, cloud services) should be substituted — and only via test fixtures, never mocks.

### Setup with DI Container

```csharp
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;

[Category("Integration")]
[Category("{Domain}")]
public class {Domain}IntegrationTests
{
    private ServiceProvider _serviceProvider = null!;
    private SomeDomainFixture _fixture = null!;

    [Before(Test)]
    public void Setup()
    {
        _fixture = new SomeDomainFixture();
        var services = new ServiceCollection();

        services.AddCyclotron{Domain}(options =>
        {
            // configure with test-appropriate settings
        });

        // Add test sinks / providers if needed
        services.AddLogging(builder => builder.AddProvider(_testSink));

        _serviceProvider = services.BuildServiceProvider();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        _serviceProvider.Dispose();
        await _fixture.DisposeAsync();
    }
}
```

- **Always** include `[After(Test)]` that disposes the `ServiceProvider` and any fixture.
- Each test gets its own container — never share a `ServiceProvider` across tests.

### Setup with File System Fixture (no DI)

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

### Pattern: Verify Service Registration

```csharp
[Test]
public void AddCyclotronTelemetry_RegistersICyclotronLogger()
{
    var logger = _serviceProvider.GetService<ICyclotronLogger>();

    logger.Should().NotBeNull();
}
```

One test per critical service registration. Use `GetService<T>()` (returns null) for should-register checks, `GetRequiredService<T>()` inside test logic that depends on resolution.

### Pattern: End-to-End Pipeline

```csharp
[Test]
public void LogInformation_WritesToTestSink()
{
    var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
    var msLogger = loggerFactory.CreateLogger("TestCategory");

    msLogger.LogInformation("Integration test message");

    _testSink.LoggedEntries.Should().Contain(e =>
        e.Level == LogLevel.Information &&
        e.Message.Contains("Integration test message"));
}
```

The pattern is: resolve → act → assert on the captured output (fixture sink, file system, etc.).

### Pattern: Concurrency

```csharp
[Test]
public async Task ParallelWrites_ToDifferentFiles_AllSucceed()
{
    var tasks = Enumerable.Range(0, 50).Select(async i =>
    {
        var filePath = _fixture.GetTempFilePath($"parallel_{i}.txt");
        await File.WriteAllTextAsync(filePath, $"Content {i}");
    });

    await Task.WhenAll(tasks);

    var files = Directory.EnumerateFiles(_fixture.RootPath, "parallel_*.txt").ToList();
    files.Should().HaveCount(50);
}
```

- Cap at 20–50 parallel tasks — enough to surface race conditions without overwhelming CI.
- Each task must use its own unique resource (file path, key, etc.).
- Assert on the final state, not on intermediate ordering.

### Pattern: Concurrent Access to Shared Resource

```csharp
[Test]
public async Task ConcurrentLogging_FromMultipleThreads_DoesNotThrow()
{
    var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();

    var tasks = Enumerable.Range(0, 50).Select(i =>
    {
        return Task.Run(() =>
        {
            var logger = loggerFactory.CreateLogger($"Thread{i}");
            logger.LogInformation($"Message from thread {i}");
        });
    });

    var act = async () => await Task.WhenAll(tasks);

    await act.Should().NotThrowAsync();
    _testSink.LoggedEntries.Should().HaveCountGreaterThanOrEqualTo(50);
}
```

---

## Reference Examples

Study these existing test files for complete, working implementations of the patterns above:

- **Unit / FileSystemAdapter**: `Cyclotron.Tests/Unit/FileSystemAdapter/FileSystemAdapterTests.cs`
- **Unit / Telemetry**: `Cyclotron.Tests/Unit/Telemetry/TelemetryAdapterTests.cs`
- **Integration / FileSystemAdapter**: `Cyclotron.Tests/Integration/FileSystemAdapter/FileSystemAdapterIntegrationTests.cs`
- **Integration / Telemetry**: `Cyclotron.Tests/Integration/Telemetry/TelemetryAdapterIntegrationTests.cs`
