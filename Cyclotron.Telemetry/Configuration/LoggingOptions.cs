using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Cyclotron.Telemetry.Configuration;

/// <summary>
/// Configuration options for logging.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// Gets or sets the minimum log level for all logging.
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// Gets or sets whether to enable caller information enrichment (file/method/line).
    /// </summary>
    public bool EnableCallerInfo { get; set; } = true;

    /// <summary>
    /// Gets or sets the output template for log messages.
    /// </summary>
    public string OutputTemplate { get; set; } =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Module}] [{MemberName}@{FilePath}:{LineNumber}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// Gets or sets the file logging options.
    /// </summary>
    public FileLoggingOptions File { get; set; } = new();

    // Future: RemoteLoggingOptions Remote { get; set; }
}

/// <summary>
/// Configuration options for file-based logging.
/// </summary>
public class FileLoggingOptions
{
    /// <summary>
    /// Gets or sets whether file logging is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the log file path. Supports placeholders like {LocalAppData}.
    /// </summary>
    public string Path { get; set; } = "{LocalAppData}/CyclotronApp/logs/app-.log";

    /// <summary>
    /// Gets or sets the rolling interval for log files.
    /// </summary>
    public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;

    /// <summary>
    /// Gets or sets the number of log files to retain.
    /// </summary>
    public int RetainedFileCountLimit { get; set; } = 3;

    /// <summary>
    /// Gets or sets the buffer size for async logging.
    /// </summary>
    public int BufferSize { get; set; } = 10000;

    /// <summary>
    /// Gets or sets the flush interval for buffered logs.
    /// </summary>
    public TimeSpan FlushInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the minimum log level for file logging.
    /// Can be different from the global minimum level.
    /// </summary>
    public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Gets or sets the maximum file size in bytes before rolling.
    /// Default is 100MB.
    /// </summary>
    public long FileSizeLimitBytes { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Gets or sets whether to roll on file size limit.
    /// </summary>
    public bool RollOnFileSizeLimit { get; set; } = true;
}
