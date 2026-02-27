using Cyclotron.Tests.Integration.Fixtures;
using AwesomeAssertions;
using Microsoft.Extensions.Logging;

namespace Cyclotron.Tests.TestHelpers;

/// <summary>
/// AwesomeAssertions extensions for telemetry validation.
/// </summary>
public static class TelemetryAssertions
{
    /// <summary>
    /// Asserts that a log message was logged at the specified level.
    /// </summary>
    public static void ShouldHaveLogged(this TelemetryTestSink sink, LogLevel level, string messageContains)
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.Level == level && e.Message.Contains(messageContains, StringComparison.OrdinalIgnoreCase),
                $"expected log with level {level} and message containing '{messageContains}'");
    }

    /// <summary>
    /// Asserts that an exception was logged.
    /// </summary>
    public static void ShouldHaveLoggedException<TException>(this TelemetryTestSink sink)
        where TException : Exception
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.Exception is TException,
                $"expected log with exception of type {typeof(TException).Name}");
    }

    /// <summary>
    /// Asserts that a log contains a specific scope property.
    /// </summary>
    public static void ShouldHaveScope(this TelemetryTestSink sink, string key, object value)
    {
        sink.LoggedEntries
            .Should()
            .Contain(e => e.State.ContainsKey(key) && Equals(e.State[key], value),
                $"expected log with scope property {key}={value}");
    }
}
