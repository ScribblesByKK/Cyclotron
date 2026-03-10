using Cyclotron.Telemetry.Configuration;
using Cyclotron.Telemetry.Logging;
using Cyclotron.Tests.Integration.Fixtures;
using Cyclotron.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Cyclotron.Extensions.DependencyInjection;

namespace Cyclotron.Tests.Integration.Telemetry;

[Category("Integration")]
[Category("Telemetry")]
public class TelemetryAdapterIntegrationTests
{
    private ServiceProvider _serviceProvider = null!;
    private TelemetryTestSink _testSink = null!;

    [Before(Test)]
    public void Setup()
    {
        _testSink = new TelemetryTestSink();
        var services = new ServiceCollection();

        services.AddCyclotronTelemetry(options =>
        {
            options.ServiceName = "TestService";
            options.ServiceVersion = "1.0.0-test";
            options.Environment = "test";
            options.DefaultModule = "test-module";
            options.Logging.File.Enabled = false;
            options.Logging.EnableCallerInfo = true;
        });

        // Add our test sink to capture log output
        services.AddLogging(builder =>
        {
            builder.AddProvider(_testSink);
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    [After(Test)]
    public void Cleanup()
    {
        _serviceProvider.Dispose();
        _testSink.Dispose();
    }

    #region Service Registration

    [Test]
    public async Task AddCyclotronTelemetry_RegistersICyclotronLogger()
    {
        var logger = _serviceProvider.GetService<ICyclotronLogger>();

        await Assert.That(logger).IsNotNull();
    }

    [Test]
    public async Task AddCyclotronTelemetry_RegistersSerilogLogger()
    {
        var serilogLogger = _serviceProvider.GetService<Serilog.ILogger>();

        await Assert.That(serilogLogger).IsNotNull();
    }

    [Test]
    public async Task AddCyclotronTelemetry_RegistersILoggerFactory()
    {
        var factory = _serviceProvider.GetService<ILoggerFactory>();

        await Assert.That(factory).IsNotNull();
    }

    #endregion

    #region Logger Configuration

    [Test]
    public async Task CyclotronLogger_HasCorrectModuleName()
    {
        var logger = _serviceProvider.GetRequiredService<ICyclotronLogger>();

        await Assert.That(logger.ModuleName).IsEqualTo("test-module");
    }

    [Test]
    public async Task CyclotronLogger_ForModule_CreatesNewLogger()
    {
        var logger = _serviceProvider.GetRequiredService<ICyclotronLogger>();

        var moduleLogger = logger.ForModule("CustomModule");

        await Assert.That(moduleLogger.ModuleName).IsEqualTo("CustomModule");
        await Assert.That(moduleLogger).IsNotSameReferenceAs(logger);
    }

    #endregion

    #region End-to-End Logging

    [Test]
    public async Task LogInformation_WritesToTestSink()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");

        msLogger.LogInformation("Integration test message");

        await Assert.That(
            _testSink.LoggedEntries.Any(e =>
                e.Level == LogLevel.Information &&
                e.Message.Contains("Integration test message")))
            .IsTrue();
    }

    [Test]
    public async Task LogError_WithException_CapturesExceptionDetails()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        var exception = new InvalidOperationException("Test exception");

        msLogger.LogError(exception, "Error occurred");

        await Assert.That(
            _testSink.LoggedEntries.Any(e =>
                e.Level == LogLevel.Error &&
                e.Exception is InvalidOperationException))
            .IsTrue();
    }

    [Test]
    public async Task LogWarning_WritesToTestSink()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");

        msLogger.LogWarning("Warning message");

        await Assert.That(
            _testSink.LoggedEntries.Any(e =>
                e.Level == LogLevel.Warning &&
                e.Message.Contains("Warning message")))
            .IsTrue();
    }

    #endregion

    #region Default Configuration

    [Test]
    public async Task AddCyclotronTelemetry_WithDefaults_Succeeds()
    {
        var services = new ServiceCollection();

        services.AddCyclotronTelemetry();

        using var sp = services.BuildServiceProvider();
        var logger = sp.GetService<ICyclotronLogger>();
        await Assert.That(logger).IsNotNull();
        await Assert.That(logger!.ModuleName).IsEqualTo("core");
    }

    #endregion

    #region TelemetryTestSink Tests

    [Test]
    public async Task TestSink_Clear_RemovesAllEntries()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Message 1");
        msLogger.LogInformation("Message 2");

        _testSink.Clear();

        await Assert.That(_testSink.LoggedEntries).IsEmpty();
    }

    [Test]
    public async Task TestSink_GetLogs_ByLevel_FiltersCorrectly()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Info message");
        msLogger.LogWarning("Warning message");
        msLogger.LogError("Error message");

        var warnings = _testSink.GetLogs(LogLevel.Warning).ToList();

        await Assert.That(warnings).Count().IsEqualTo(1);
        await Assert.That(warnings[0].Message).Contains("Warning message");
    }

    [Test]
    public async Task TestSink_GetLogs_ByCategoryContains_FiltersCorrectly()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Test message");

        var logs = _testSink.GetLogs("TestCategory").ToList();

        await Assert.That(logs).IsNotEmpty();
    }

    #endregion

    #region Concurrent Logging

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

        await Assert.That(act).ThrowsNothing();
        await Assert.That(_testSink.LoggedEntries.Count).IsGreaterThanOrEqualTo(50);
    }

    #endregion

    #region TelemetryAssertions Helper Tests

    [Test]
    public async Task ShouldHaveLogged_FindsMatchingEntry()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Expected message");

        await _testSink.ShouldHaveLogged(LogLevel.Information, "Expected message");
    }

    [Test]
    public async Task ShouldHaveLoggedException_FindsMatchingException()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogError(new ArgumentException("test"), "Error");

        await _testSink.ShouldHaveLoggedException<ArgumentException>();
    }

    #endregion
}
