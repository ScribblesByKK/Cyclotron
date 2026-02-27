using Cyclotron.Telemetry.Configuration;
using Cyclotron.Telemetry.Logging;
using AwesomeAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Serilog;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Cyclotron.Tests.Unit.Telemetry;

[Category("Unit")]
[Category("Telemetry")]
public class TelemetryAdapterTests
{
    private Serilog.ILogger _serilogLogger = null!;
    private ILogger<CyclotronLogger> _msLogger = null!;
    private IOptions<CyclotronTelemetryOptions> _options = null!;
    private CyclotronTelemetryOptions _telemetryOptions = null!;

    [Before(Test)]
    public void Setup()
    {
        _serilogLogger = Substitute.For<Serilog.ILogger>();
        _msLogger = Substitute.For<ILogger<CyclotronLogger>>();
        _telemetryOptions = new CyclotronTelemetryOptions();
        _options = Options.Create(_telemetryOptions);

        // Make ForContext return the same mock so Write calls are trackable
        _serilogLogger.ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>()).Returns(_serilogLogger);
    }

    #region Constructor & Validation

    [Test]
    public void Constructor_WithValidParameters_Succeeds()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.Should().NotBeNull();
    }

    [Test]
    public void Constructor_SetsDefaultModuleName()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.ModuleName.Should().Be("core");
    }

    [Test]
    public void Constructor_WithCustomDefaultModule_SetsModuleName()
    {
        _telemetryOptions.DefaultModule = "CustomModule";
        var options = Options.Create(_telemetryOptions);

        var logger = new CyclotronLogger(options, _serilogLogger, _msLogger);

        logger.ModuleName.Should().Be("CustomModule");
    }

    #endregion

    #region ForModule

    [Test]
    public void ForModule_ReturnsNewLoggerInstance()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        var moduleLogger = logger.ForModule("TestModule");

        moduleLogger.Should().NotBeSameAs(logger);
    }

    [Test]
    public void ForModule_SetsCorrectModuleName()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        var moduleLogger = logger.ForModule("TestModule");

        moduleLogger.ModuleName.Should().Be("TestModule");
    }

    [Test]
    public void ForModule_DoesNotAffectOriginalLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        _ = logger.ForModule("TestModule");

        logger.ModuleName.Should().Be("core");
    }

    #endregion

    #region LogDebug

    [Test]
    public void LogDebug_CallsSerilogWithDebugLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogDebug("Test debug message");

        _serilogLogger.Received(1).ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>());
        _serilogLogger.Received(1).Write(LogEventLevel.Debug, (Exception?)null, "Test debug message");
    }

    [Test]
    public void LogDebug_WithCallerInfoDisabled_LogsWithoutEnrichment()
    {
        _telemetryOptions.Logging.EnableCallerInfo = false;
        var options = Options.Create(_telemetryOptions);
        var logger = new CyclotronLogger(options, _serilogLogger, _msLogger);

        logger.LogDebug("Test message");

        _serilogLogger.DidNotReceive().ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>());
        _serilogLogger.Received(1).Write(LogEventLevel.Debug, (Exception?)null, "Test message");
    }

    #endregion

    #region LogInformation

    [Test]
    public void LogInformation_CallsSerilogWithInformationLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogInformation("Test info message");

        _serilogLogger.Received(1).Write(LogEventLevel.Information, (Exception?)null, "Test info message");
    }

    [Test]
    public void LogInformation_WithCallerInfoEnabled_EnrichesLog()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogInformation("Test info");

        _serilogLogger.Received(1).ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>());
    }

    #endregion

    #region LogWarning

    [Test]
    public void LogWarning_CallsSerilogWithWarningLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogWarning("Test warning");

        _serilogLogger.Received(1).Write(LogEventLevel.Warning, (Exception?)null, "Test warning");
    }

    [Test]
    public void LogWarning_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        var exception = new InvalidOperationException("Test exception");

        logger.LogWarning("Warning with exception", exception);

        _serilogLogger.Received(1).Write(LogEventLevel.Warning, exception, "Warning with exception");
    }

    #endregion

    #region LogError

    [Test]
    public void LogError_CallsSerilogWithErrorLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogError("Test error");

        _serilogLogger.Received(1).Write(LogEventLevel.Error, (Exception?)null, "Test error");
    }

    [Test]
    public void LogError_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        var exception = new ArgumentException("Bad argument");

        logger.LogError("Error occurred", exception);

        _serilogLogger.Received(1).Write(LogEventLevel.Error, exception, "Error occurred");
    }

    #endregion

    #region LogCritical

    [Test]
    public void LogCritical_CallsSerilogWithFatalLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);

        logger.LogCritical("Critical failure");

        _serilogLogger.Received(1).Write(LogEventLevel.Fatal, (Exception?)null, "Critical failure");
    }

    [Test]
    public void LogCritical_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        var exception = new OutOfMemoryException();

        logger.LogCritical("Out of memory", exception);

        _serilogLogger.Received(1).Write(LogEventLevel.Fatal, exception, "Out of memory");
    }

    #endregion

    #region ILogger Implementation - BeginScope

    [Test]
    public void BeginScope_DelegatesToMsLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        var scope = Substitute.For<IDisposable>();
        _msLogger.BeginScope(Arg.Any<object>()).Returns(scope);

        var result = logger.BeginScope(new { CorrelationId = "test-123" });

        result.Should().BeSameAs(scope);
    }

    #endregion

    #region ILogger Implementation - IsEnabled

    [Test]
    public void IsEnabled_DelegatesToMsLogger_WhenTrue()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        _msLogger.IsEnabled(LogLevel.Information).Returns(true);

        logger.IsEnabled(LogLevel.Information).Should().BeTrue();
    }

    [Test]
    public void IsEnabled_DelegatesToMsLogger_WhenFalse()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        _msLogger.IsEnabled(LogLevel.Debug).Returns(false);

        logger.IsEnabled(LogLevel.Debug).Should().BeFalse();
    }

    #endregion

    #region ILogger Implementation - Log<TState>

    [Test]
    public void Log_DelegatesToMsLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLogger, _msLogger);
        var eventId = new EventId(1, "TestEvent");

        logger.Log(LogLevel.Information, eventId, "state", null, (s, e) => s.ToString()!);

        _msLogger.Received(1).Log(
            LogLevel.Information,
            eventId,
            "state",
            null,
            Arg.Any<Func<string, Exception?, string>>());
    }

    #endregion

    #region Configuration Defaults

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

    [Test]
    public void LoggingOptions_HasCorrectDefaults()
    {
        var options = new LoggingOptions();

        options.MinimumLevel.Should().Be(LogLevel.Information);
        options.EnableCallerInfo.Should().BeTrue();
        options.OutputTemplate.Should().NotBeNullOrEmpty();
        options.File.Should().NotBeNull();
    }

    [Test]
    public void FileLoggingOptions_HasCorrectDefaults()
    {
        var options = new FileLoggingOptions();

        options.Enabled.Should().BeTrue();
        options.Path.Should().Contain("CyclotronApp");
        options.RetainedFileCountLimit.Should().Be(3);
        options.BufferSize.Should().Be(10000);
        options.FlushInterval.Should().Be(TimeSpan.FromSeconds(30));
        options.MinimumLevel.Should().Be(LogLevel.Debug);
        options.FileSizeLimitBytes.Should().Be(10 * 1024 * 1024);
        options.RollOnFileSizeLimit.Should().BeTrue();
    }

    #endregion

    #region CallerInfo Enrichment

    [Test]
    public void LogDebug_WithCallerInfoEnabled_UsesForContext()
    {
        _telemetryOptions.Logging.EnableCallerInfo = true;
        var options = Options.Create(_telemetryOptions);
        var logger = new CyclotronLogger(options, _serilogLogger, _msLogger);

        logger.LogDebug("test");

        _serilogLogger.Received(1).ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>());
    }

    [Test]
    public void LogInformation_WithCallerInfoDisabled_SkipsForContext()
    {
        _telemetryOptions.Logging.EnableCallerInfo = false;
        var options = Options.Create(_telemetryOptions);
        var logger = new CyclotronLogger(options, _serilogLogger, _msLogger);

        logger.LogInformation("test");

        _serilogLogger.DidNotReceive().ForContext(Arg.Any<Serilog.Core.ILogEventEnricher>());
    }

    #endregion
}
