using Cyclotron.Tests.Integration.Fixtures;
using Microsoft.Extensions.Logging;

namespace Cyclotron.Tests.TestHelpers;

/// <summary>
/// Assertion extensions for telemetry validation.
/// </summary>
public static class TelemetryAssertions
{
    /// <summary>
    /// Asserts that a log message was logged at the specified level.
    /// </summary>
    public static async Task ShouldHaveLogged(this TelemetryTestSink sink, LogLevel level, string messageContains)
    {
        await Assert.That(
            sink.LoggedEntries.Any(e => e.Level == level && e.Message.Contains(messageContains, StringComparison.OrdinalIgnoreCase)))
            .IsTrue()
            .Because($"expected log with level {level} and message containing '{messageContains}'");
    }

    /// <summary>
    /// Asserts that an exception was logged.
    /// </summary>
    public static async Task ShouldHaveLoggedException<TException>(this TelemetryTestSink sink)
        where TException : Exception
    {
        await Assert.That(
            sink.LoggedEntries.Any(e => e.Exception is TException))
            .IsTrue()
            .Because($"expected log with exception of type {typeof(TException).Name}");
    }

    /// <summary>
    /// Asserts that a log contains a specific scope property.
    /// </summary>
    public static async Task ShouldHaveScope(this TelemetryTestSink sink, string key, object value)
    {
        await Assert.That(
            sink.LoggedEntries.Any(e => e.State.ContainsKey(key) && Equals(e.State[key], value)))
            .IsTrue()
            .Because($"expected log with scope property {key}={value}");
    }
}
