using Cyclotron.Telemetry.Configuration;
using Cyclotron.Telemetry.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Cyclotron.Tests.Unit.Telemetry;

[Category("Unit")]
[Category("Telemetry")]
public class TelemetryAdapterTests
{
    private Mock<Serilog.ILogger> _serilogLoggerMock = null!;
    private Mock<ILogger<CyclotronLogger>> _msLoggerMock = null!;
    private IOptions<CyclotronTelemetryOptions> _options = null!;
    private CyclotronTelemetryOptions _telemetryOptions = null!;

    [Before(Test)]
    public void Setup()
    {
        _serilogLoggerMock = Mock.Of<Serilog.ILogger>();
        _msLoggerMock = Mock.Of<ILogger<CyclotronLogger>>();
        _telemetryOptions = new CyclotronTelemetryOptions();
        _options = Options.Create(_telemetryOptions);

        // Make ForContext return the same mock so Write calls are trackable
        _serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).Returns(_serilogLoggerMock.Object);
    }

    #region Constructor & Validation

    [Test]
    public async Task Constructor_WithValidParameters_Succeeds()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        await Assert.That(logger).IsNotNull();
    }

    [Test]
    public async Task Constructor_SetsDefaultModuleName()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        await Assert.That(logger.ModuleName).IsEqualTo("core");
    }

    [Test]
    public async Task Constructor_WithCustomDefaultModule_SetsModuleName()
    {
        _telemetryOptions.DefaultModule = "CustomModule";
        var options = Options.Create(_telemetryOptions);

        var logger = new CyclotronLogger(options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        await Assert.That(logger.ModuleName).IsEqualTo("CustomModule");
    }

    #endregion

    #region ForModule

    [Test]
    public async Task ForModule_ReturnsNewLoggerInstance()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        var moduleLogger = logger.ForModule("TestModule");

        await Assert.That(moduleLogger).IsNotSameReferenceAs(logger);
    }

    [Test]
    public async Task ForModule_SetsCorrectModuleName()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        var moduleLogger = logger.ForModule("TestModule");

        await Assert.That(moduleLogger.ModuleName).IsEqualTo("TestModule");
    }

    [Test]
    public async Task ForModule_DoesNotAffectOriginalLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        _ = logger.ForModule("TestModule");

        await Assert.That(logger.ModuleName).IsEqualTo("core");
    }

    #endregion

    #region LogDebug

    [Test]
    public async Task LogDebug_CallsSerilogWithDebugLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogDebug("Test debug message");

        _serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).WasCalled(Times.Once);
        _serilogLoggerMock.Write(LogEventLevel.Debug, IsNull<Exception>(), "Test debug message").WasCalled(Times.Once);
    }

    [Test]
    public async Task LogDebug_WithCallerInfoDisabled_LogsWithoutEnrichment()
    {
        _telemetryOptions.Logging.EnableCallerInfo = false;
        var options = Options.Create(_telemetryOptions);
        var serilogLoggerMock = Mock.Of<Serilog.ILogger>();
        var logger = new CyclotronLogger(options, serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogDebug("Test message");

        serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).WasNeverCalled();
        serilogLoggerMock.Write(LogEventLevel.Debug, IsNull<Exception>(), "Test message").WasCalled(Times.Once);
    }

    #endregion

    #region LogInformation

    [Test]
    public async Task LogInformation_CallsSerilogWithInformationLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogInformation("Test info message");

        _serilogLoggerMock.Write(LogEventLevel.Information, IsNull<Exception>(), "Test info message").WasCalled(Times.Once);
    }

    [Test]
    public async Task LogInformation_WithCallerInfoEnabled_EnrichesLog()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogInformation("Test info");

        _serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).WasCalled(Times.Once);
    }

    #endregion

    #region LogWarning

    [Test]
    public async Task LogWarning_CallsSerilogWithWarningLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogWarning("Test warning");

        _serilogLoggerMock.Write(LogEventLevel.Warning, IsNull<Exception>(), "Test warning").WasCalled(Times.Once);
    }

    [Test]
    public async Task LogWarning_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        var exception = new InvalidOperationException("Test exception");

        logger.LogWarning("Warning with exception", exception);

        _serilogLoggerMock.Write(LogEventLevel.Warning, exception, "Warning with exception").WasCalled(Times.Once);
    }

    #endregion

    #region LogError

    [Test]
    public async Task LogError_CallsSerilogWithErrorLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogError("Test error");

        _serilogLoggerMock.Write(LogEventLevel.Error, IsNull<Exception>(), "Test error").WasCalled(Times.Once);
    }

    [Test]
    public async Task LogError_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        var exception = new ArgumentException("Bad argument");

        logger.LogError("Error occurred", exception);

        _serilogLoggerMock.Write(LogEventLevel.Error, exception, "Error occurred").WasCalled(Times.Once);
    }

    #endregion

    #region LogCritical

    [Test]
    public async Task LogCritical_CallsSerilogWithFatalLevel()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogCritical("Critical failure");

        _serilogLoggerMock.Write(LogEventLevel.Fatal, IsNull<Exception>(), "Critical failure").WasCalled(Times.Once);
    }

    [Test]
    public async Task LogCritical_WithException_PassesExceptionToSerilog()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        var exception = new OutOfMemoryException();

        logger.LogCritical("Out of memory", exception);

        _serilogLoggerMock.Write(LogEventLevel.Fatal, exception, "Out of memory").WasCalled(Times.Once);
    }

    #endregion

    #region ILogger Implementation - BeginScope

    [Test]
    public async Task BeginScope_DelegatesToMsLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        var scopeMock = Mock.Of<IDisposable>();
        _msLoggerMock.BeginScope(Any<object>()).Returns(scopeMock.Object);

        var result = logger.BeginScope(new { CorrelationId = "test-123" });

        await Assert.That(result).IsSameReferenceAs(scopeMock.Object);
    }

    #endregion

    #region ILogger Implementation - IsEnabled

    [Test]
    public async Task IsEnabled_DelegatesToMsLogger_WhenTrue()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        _msLoggerMock.IsEnabled(LogLevel.Information).Returns(true);

        await Assert.That(logger.IsEnabled(LogLevel.Information)).IsTrue();
    }

    [Test]
    public async Task IsEnabled_DelegatesToMsLogger_WhenFalse()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        _msLoggerMock.IsEnabled(LogLevel.Debug).Returns(false);

        await Assert.That(logger.IsEnabled(LogLevel.Debug)).IsFalse();
    }

    #endregion

    #region ILogger Implementation - Log<TState>

    [Test]
    public async Task Log_DelegatesToMsLogger()
    {
        var logger = new CyclotronLogger(_options, _serilogLoggerMock.Object, _msLoggerMock.Object);
        var eventId = new EventId(1, "TestEvent");

        logger.Log(LogLevel.Information, eventId, "state", null, (s, e) => s.ToString()!);

        _msLoggerMock.Log(Any<LogLevel>(), Any<EventId>(), Any<string>(), Any<Exception?>(), Any<Func<string, Exception?, string>>()).WasCalled(Times.Once);
    }

    #endregion

    #region Configuration Defaults

    [Test]
    public async Task CyclotronTelemetryOptions_HasCorrectDefaults()
    {
        var options = new CyclotronTelemetryOptions();

        await Assert.That(options.ServiceName).IsEqualTo("CyclotronApp");
        await Assert.That(options.ServiceVersion).IsEqualTo("1.0.0");
        await Assert.That(options.Environment).IsEqualTo("development");
        await Assert.That(options.DefaultModule).IsEqualTo("core");
        await Assert.That(options.Logging).IsNotNull();
    }

    [Test]
    public async Task LoggingOptions_HasCorrectDefaults()
    {
        var options = new LoggingOptions();

        await Assert.That(options.MinimumLevel).IsEqualTo(LogLevel.Information);
        await Assert.That(options.EnableCallerInfo).IsTrue();
        await Assert.That(options.OutputTemplate).IsNotEmpty();
        await Assert.That(options.File).IsNotNull();
    }

    [Test]
    public async Task FileLoggingOptions_HasCorrectDefaults()
    {
        var options = new FileLoggingOptions();

        await Assert.That(options.Enabled).IsTrue();
        await Assert.That(options.Path).Contains("CyclotronApp");
        await Assert.That(options.RetainedFileCountLimit).IsEqualTo(3);
        await Assert.That(options.BufferSize).IsEqualTo(10000);
        await Assert.That(options.FlushInterval).IsEqualTo(TimeSpan.FromSeconds(30));
        await Assert.That(options.MinimumLevel).IsEqualTo(LogLevel.Debug);
        await Assert.That(options.FileSizeLimitBytes).IsEqualTo(10 * 1024 * 1024);
        await Assert.That(options.RollOnFileSizeLimit).IsTrue();
    }

    #endregion

    #region CallerInfo Enrichment

    [Test]
    public async Task LogDebug_WithCallerInfoEnabled_UsesForContext()
    {
        _telemetryOptions.Logging.EnableCallerInfo = true;
        var options = Options.Create(_telemetryOptions);
        var logger = new CyclotronLogger(options, _serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogDebug("test");

        _serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).WasCalled(Times.Once);
    }

    [Test]
    public async Task LogInformation_WithCallerInfoDisabled_SkipsForContext()
    {
        _telemetryOptions.Logging.EnableCallerInfo = false;
        var options = Options.Create(_telemetryOptions);
        var serilogLoggerMock = Mock.Of<Serilog.ILogger>();
        var logger = new CyclotronLogger(options, serilogLoggerMock.Object, _msLoggerMock.Object);

        logger.LogInformation("test");

        serilogLoggerMock.ForContext(Any<Serilog.Core.ILogEventEnricher>()).WasNeverCalled();
    }

    #endregion
}
