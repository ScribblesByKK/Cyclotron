using System.Runtime.CompilerServices;
using Cyclotron.Telemetry.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Cyclotron.Telemetry.Logging;

/// <summary>
/// Default implementation of <see cref="ICyclotronLogger"/> with Serilog backend.
/// Provides automatic caller information enrichment and module-based tagging.
/// </summary>
public sealed class CyclotronLogger : ICyclotronLogger
{
    private readonly Serilog.ILogger _serilogLogger;
    private readonly ILogger _msLogger;
    private readonly CyclotronTelemetryOptions _options;

    /// <inheritdoc/>
    public string ModuleName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclotronLogger"/> class.
    /// </summary>
    /// <param name="options">The telemetry options.</param>
    /// <param name="serilogLogger">The Serilog logger instance.</param>
    /// <param name="msLogger">The Microsoft.Extensions.Logging logger instance.</param>
    public CyclotronLogger(
        IOptions<CyclotronTelemetryOptions> options,
        Serilog.ILogger serilogLogger,
        ILogger<CyclotronLogger> msLogger)
        : this(options.Value, serilogLogger, msLogger, options.Value.DefaultModule)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyclotronLogger"/> class with a specific module name.
    /// </summary>
    /// <param name="options">The telemetry options.</param>
    /// <param name="serilogLogger">The Serilog logger instance.</param>
    /// <param name="msLogger">The Microsoft.Extensions.Logging logger instance.</param>
    /// <param name="moduleName">The module name to associate with this logger instance.</param>
    private CyclotronLogger(
        CyclotronTelemetryOptions options,
        Serilog.ILogger serilogLogger,
        ILogger msLogger,
        string moduleName)
    {
        _options = options;
        _serilogLogger = serilogLogger;
        _msLogger = msLogger;
        ModuleName = moduleName;
    }

    /// <inheritdoc/>
    public ICyclotronLogger ForModule(string moduleName)
    {
        return new CyclotronLogger(_options, _serilogLogger, _msLogger, moduleName);
    }

    /// <inheritdoc/>
    public void LogDebug(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        LogWithCallerInfo(LogEventLevel.Debug, message, null, memberName, filePath, lineNumber);
    }

    /// <inheritdoc/>
    public void LogInformation(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        LogWithCallerInfo(LogEventLevel.Information, message, null, memberName, filePath, lineNumber);
    }

    /// <inheritdoc/>
    public void LogWarning(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        LogWithCallerInfo(LogEventLevel.Warning, message, exception, memberName, filePath, lineNumber);
    }

    /// <inheritdoc/>
    public void LogError(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        LogWithCallerInfo(LogEventLevel.Error, message, exception, memberName, filePath, lineNumber);
    }

    /// <inheritdoc/>
    public void LogCritical(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        LogWithCallerInfo(LogEventLevel.Fatal, message, exception, memberName, filePath, lineNumber);
    }

    /// <summary>
    /// Logs a message with caller information enrichment.
    /// </summary>
    /// <param name="level">The log event level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="exception">Optional exception to log.</param>
    /// <param name="memberName">The calling member name.</param>
    /// <param name="filePath">The calling file path.</param>
    /// <param name="lineNumber">The calling line number.</param>
    /// <remarks>
    /// If caller info enrichment is disabled in options, logs without enrichment.
    /// Otherwise, enriches the log with module, member name, file path, and line number information.
    /// </remarks>
    private void LogWithCallerInfo(
        LogEventLevel level,
        string message,
        Exception? exception,
        string memberName,
        string filePath,
        int lineNumber)
    {
        if (!_options.Logging.EnableCallerInfo)
        {
            _serilogLogger.Write(level, exception, message);
            return;
        }

        var enricher = new CallerInfoEnricher(ModuleName, memberName, filePath, lineNumber);

        _serilogLogger
            .ForContext(enricher)
            .Write(level, exception, message);
    }

    #region ILogger Implementation

    /// <inheritdoc/>
    /// <remarks>
    /// Delegates to the underlying Microsoft.Extensions.Logging ILogger implementation.
    /// </remarks>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _msLogger.BeginScope(state);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Delegates to the underlying Microsoft.Extensions.Logging ILogger implementation.
    /// </remarks>
    public bool IsEnabled(LogLevel logLevel)
    {
        return _msLogger.IsEnabled(logLevel);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Delegates to the underlying Microsoft.Extensions.Logging ILogger implementation.
    /// </remarks>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _msLogger.Log(logLevel, eventId, state, exception, formatter);
    }

    #endregion
}
