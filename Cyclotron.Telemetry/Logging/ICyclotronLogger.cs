using System.Runtime.CompilerServices;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Cyclotron.Telemetry.Logging;

/// <summary>
/// Cyclotron logger with automatic caller information enrichment.
/// Fully compatible with Microsoft.Extensions.Logging.ILogger.
/// </summary>
public interface ICyclotronLogger : ILogger
{
    /// <summary>
    /// Gets the module name for this logger instance.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Logs a debug message with caller information.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="memberName">The calling member name (auto-populated).</param>
    /// <param name="filePath">The calling file path (auto-populated).</param>
    /// <param name="lineNumber">The calling line number (auto-populated).</param>
    void LogDebug(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Logs an information message with caller information.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="memberName">The calling member name (auto-populated).</param>
    /// <param name="filePath">The calling file path (auto-populated).</param>
    /// <param name="lineNumber">The calling line number (auto-populated).</param>
    void LogInformation(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Logs a warning message with caller information.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="exception">Optional exception to log.</param>
    /// <param name="memberName">The calling member name (auto-populated).</param>
    /// <param name="filePath">The calling file path (auto-populated).</param>
    /// <param name="lineNumber">The calling line number (auto-populated).</param>
    void LogWarning(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Logs an error message with caller information.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="exception">Optional exception to log.</param>
    /// <param name="memberName">The calling member name (auto-populated).</param>
    /// <param name="filePath">The calling file path (auto-populated).</param>
    /// <param name="lineNumber">The calling line number (auto-populated).</param>
    void LogError(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Logs a critical message with caller information.
    /// </summary>
    /// <param name="message">The log message.</param>
    /// <param name="exception">Optional exception to log.</param>
    /// <param name="memberName">The calling member name (auto-populated).</param>
    /// <param name="filePath">The calling file path (auto-populated).</param>
    /// <param name="lineNumber">The calling line number (auto-populated).</param>
    void LogCritical(
        string message,
        Exception? exception = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Creates a child logger for a specific module.
    /// </summary>
    /// <param name="moduleName">The module name to tag logs with.</param>
    /// <returns>A new logger instance scoped to the specified module.</returns>
    ICyclotronLogger ForModule(string moduleName);
}
