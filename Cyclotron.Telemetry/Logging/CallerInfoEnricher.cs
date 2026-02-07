namespace Cyclotron.Telemetry.Logging;

/// <summary>
/// Serilog enricher for adding caller information to log events.
/// AOT-safe implementation using CallerMemberName, CallerFilePath, and CallerLineNumber attributes.
/// </summary>
internal sealed class CallerInfoEnricher : ILogEventEnricher
{
    private readonly string _memberName;
    private readonly string _filePath;
    private readonly int _lineNumber;
    private readonly string _moduleName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallerInfoEnricher"/> class.
    /// </summary>
    /// <param name="moduleName">The module name for this log entry.</param>
    /// <param name="memberName">The calling member name.</param>
    /// <param name="filePath">The full path of the source file.</param>
    /// <param name="lineNumber">The line number in the source file.</param>
    public CallerInfoEnricher(
        string moduleName,
        string memberName,
        string filePath,
        int lineNumber)
    {
        _moduleName = moduleName;
        _memberName = memberName;
        _filePath = Path.GetFileName(filePath);
        _lineNumber = lineNumber;
    }

    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Module", _moduleName));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("MemberName", _memberName));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FilePath", _filePath));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LineNumber", _lineNumber));
    }
}
