using Cyclotron.Telemetry.Configuration;
using Cyclotron.Telemetry.Logging;
using Cyclotron.Tests.Integration.Fixtures;
using Cyclotron.Tests.TestHelpers;
using FluentAssertions;
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
    public void AddCyclotronTelemetry_RegistersICyclotronLogger()
    {
        var logger = _serviceProvider.GetService<ICyclotronLogger>();

        logger.Should().NotBeNull();
    }

    [Test]
    public void AddCyclotronTelemetry_RegistersSerilogLogger()
    {
        var serilogLogger = _serviceProvider.GetService<Serilog.ILogger>();

        serilogLogger.Should().NotBeNull();
    }

    [Test]
    public void AddCyclotronTelemetry_RegistersILoggerFactory()
    {
        var factory = _serviceProvider.GetService<ILoggerFactory>();

        factory.Should().NotBeNull();
    }

    #endregion

    #region Logger Configuration

    [Test]
    public void CyclotronLogger_HasCorrectModuleName()
    {
        var logger = _serviceProvider.GetRequiredService<ICyclotronLogger>();

        logger.ModuleName.Should().Be("test-module");
    }

    [Test]
    public void CyclotronLogger_ForModule_CreatesNewLogger()
    {
        var logger = _serviceProvider.GetRequiredService<ICyclotronLogger>();

        var moduleLogger = logger.ForModule("CustomModule");

        moduleLogger.ModuleName.Should().Be("CustomModule");
        moduleLogger.Should().NotBeSameAs(logger);
    }

    #endregion

    #region End-to-End Logging

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

    [Test]
    public void LogError_WithException_CapturesExceptionDetails()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        var exception = new InvalidOperationException("Test exception");

        msLogger.LogError(exception, "Error occurred");

        _testSink.LoggedEntries.Should().Contain(e =>
            e.Level == LogLevel.Error &&
            e.Exception is InvalidOperationException);
    }

    [Test]
    public void LogWarning_WritesToTestSink()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");

        msLogger.LogWarning("Warning message");

        _testSink.LoggedEntries.Should().Contain(e =>
            e.Level == LogLevel.Warning &&
            e.Message.Contains("Warning message"));
    }

    #endregion

    #region Default Configuration

    [Test]
    public void AddCyclotronTelemetry_WithDefaults_Succeeds()
    {
        var services = new ServiceCollection();

        services.AddCyclotronTelemetry();

        using var sp = services.BuildServiceProvider();
        var logger = sp.GetService<ICyclotronLogger>();
        logger.Should().NotBeNull();
        logger!.ModuleName.Should().Be("core");
    }

    #endregion

    #region TelemetryTestSink Tests

    [Test]
    public void TestSink_Clear_RemovesAllEntries()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Message 1");
        msLogger.LogInformation("Message 2");

        _testSink.Clear();

        _testSink.LoggedEntries.Should().BeEmpty();
    }

    [Test]
    public void TestSink_GetLogs_ByLevel_FiltersCorrectly()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Info message");
        msLogger.LogWarning("Warning message");
        msLogger.LogError("Error message");

        var warnings = _testSink.GetLogs(LogLevel.Warning).ToList();

        warnings.Should().ContainSingle();
        warnings[0].Message.Should().Contain("Warning message");
    }

    [Test]
    public void TestSink_GetLogs_ByCategoryContains_FiltersCorrectly()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Test message");

        var logs = _testSink.GetLogs("TestCategory").ToList();

        logs.Should().NotBeEmpty();
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

        await act.Should().NotThrowAsync();
        _testSink.LoggedEntries.Should().HaveCountGreaterThanOrEqualTo(50);
    }

    #endregion

    #region TelemetryAssertions Helper Tests

    [Test]
    public void ShouldHaveLogged_FindsMatchingEntry()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogInformation("Expected message");

        _testSink.ShouldHaveLogged(LogLevel.Information, "Expected message");
    }

    [Test]
    public void ShouldHaveLoggedException_FindsMatchingException()
    {
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var msLogger = loggerFactory.CreateLogger("TestCategory");
        msLogger.LogError(new ArgumentException("test"), "Error");

        _testSink.ShouldHaveLoggedException<ArgumentException>();
    }

    #endregion
}
