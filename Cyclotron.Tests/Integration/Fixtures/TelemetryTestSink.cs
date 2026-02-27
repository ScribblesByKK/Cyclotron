using Microsoft.Extensions.Logging;

namespace Cyclotron.Tests.Integration.Fixtures;

/// <summary>
/// In-memory logger provider for capturing log messages in tests.
/// </summary>
public class TelemetryTestSink : ILoggerProvider
{
    private readonly List<LogEntry> _logEntries = new();
    private readonly object _lock = new();

    public IReadOnlyList<LogEntry> LoggedEntries
    {
        get
        {
            lock (_lock)
            {
                return _logEntries.ToList();
            }
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(this, categoryName);
    }

    internal void AddLog(LogLevel level, string category, string message, Exception? exception, IReadOnlyDictionary<string, object?> state)
    {
        lock (_lock)
        {
            _logEntries.Add(new LogEntry(level, category, message, exception, state));
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _logEntries.Clear();
        }
    }

    public IEnumerable<LogEntry> GetLogs(LogLevel level)
    {
        return LoggedEntries.Where(e => e.Level == level);
    }

    public IEnumerable<LogEntry> GetLogs(string categoryContains)
    {
        return LoggedEntries.Where(e => e.Category.Contains(categoryContains, StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose() { }

    private class TestLogger : ILogger
    {
        private readonly TelemetryTestSink _sink;
        private readonly string _categoryName;

        public TestLogger(TelemetryTestSink sink, string categoryName)
        {
            _sink = sink;
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new NoOpDisposable();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            var stateDict = state as IReadOnlyDictionary<string, object?> ?? new Dictionary<string, object?>();
            _sink.AddLog(logLevel, _categoryName, message, exception, stateDict);
        }

        private class NoOpDisposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}

public record LogEntry(
    LogLevel Level,
    string Category,
    string Message,
    Exception? Exception,
    IReadOnlyDictionary<string, object?> State
);
