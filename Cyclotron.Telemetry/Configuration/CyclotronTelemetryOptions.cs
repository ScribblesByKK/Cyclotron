namespace Cyclotron.Telemetry.Configuration;

/// <summary>
/// Root configuration options for Cyclotron Telemetry.
/// Provides centralized configuration for all telemetry features including logging, tracing, and metrics.
/// </summary>
/// <remarks>
/// This class is typically configured during application startup via dependency injection.
/// All properties have sensible defaults suitable for development environments.
/// </remarks>
public class CyclotronTelemetryOptions
{
    /// <summary>
    /// Gets or sets the service name for telemetry identification.
    /// Used to identify the service across all telemetry events and logs.
    /// </summary>
    /// <value>Default value is "CyclotronApp".</value>
    public string ServiceName { get; set; } = "CyclotronApp";

    /// <summary>
    /// Gets or sets the service version.
    /// Used for tracking telemetry across different application versions.
    /// </summary>
    /// <value>Default value is "1.0.0".</value>
    public string ServiceVersion { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets the environment name (e.g., development, staging, production).
    /// Used to differentiate telemetry behavior and log levels across deployment environments.
    /// </summary>
    /// <value>Default value is "development".</value>
    public string Environment { get; set; } = "development";

    /// <summary>
    /// Gets or sets the default module name for untagged telemetry.
    /// When a logger is created without specifying a module, this default is used.
    /// </summary>
    /// <value>Default value is "core".</value>
    public string DefaultModule { get; set; } = "core";

    /// <summary>
    /// Gets or sets the logging configuration options.
    /// Contains settings for log levels, file output, and enrichment behavior.
    /// </summary>
    /// <value>A new <see cref="LoggingOptions"/> instance with default values.</value>
    public LoggingOptions Logging { get; set; } = new();

    // Future: TracingOptions, MetricsOptions, etc.
}
