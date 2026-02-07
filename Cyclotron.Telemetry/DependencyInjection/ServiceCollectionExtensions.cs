using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130, S1200
namespace Cyclotron.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for adding Cyclotron Telemetry services to the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Cyclotron Telemetry services to the DI container with code-based configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Action to configure telemetry options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCyclotronTelemetry(
        this IServiceCollection services,
        Action<CyclotronTelemetryOptions> configure)
    {
        var options = new CyclotronTelemetryOptions();
        configure(options);

        services.Configure(configure);

        ConfigureLogging(services, options);

        return services;
    }

    /// <summary>
    /// Adds Cyclotron Telemetry services to the DI container with default configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCyclotronTelemetry(this IServiceCollection services)
    {
        return services.AddCyclotronTelemetry(_ => { });
    }

    /// <summary>
    /// Configures Serilog and Microsoft.Extensions.Logging based on telemetry options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="options">The telemetry options containing logging configuration.</param>
    /// <remarks>
    /// Sets up Serilog with enrichers for service metadata and optional file sink if enabled.
    /// Registers both Serilog and Microsoft.Extensions.Logging providers in the DI container.
    /// </remarks>
    private static void ConfigureLogging(IServiceCollection services, CyclotronTelemetryOptions options)
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Is(ConvertLogLevel(options.Logging.MinimumLevel))
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProperty("ServiceName", options.ServiceName)
            .Enrich.WithProperty("ServiceVersion", options.ServiceVersion)
            .Enrich.WithProperty("Environment", options.Environment);

        if (options.Logging.File.Enabled)
        {
            ConfigureFileSink(loggerConfig, options);
        }

        var serilogLogger = loggerConfig.CreateLogger();
        Log.Logger = serilogLogger;

        services.AddSingleton<Serilog.ILogger>(serilogLogger);

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(serilogLogger, dispose: true);
            loggingBuilder.SetMinimumLevel(options.Logging.MinimumLevel);
        });

        services.AddSingleton<ICyclotronLogger, CyclotronLogger>();
    }

    /// <summary>
    /// Configures the file sink for Serilog logging.
    /// </summary>
    /// <param name="loggerConfig">The Serilog logger configuration to modify.</param>
    /// <param name="options">The telemetry options containing file logging settings.</param>
    /// <remarks>
    /// Configures asynchronous file logging with rolling intervals, file size limits, and retention policies.
    /// Automatically creates the log directory if it does not exist.
    /// </remarks>
    private static void ConfigureFileSink(LoggerConfiguration loggerConfig, CyclotronTelemetryOptions options)
    {
        var fileOptions = options.Logging.File;
        var logPath = ResolvePath(fileOptions.Path, options.ServiceName);

        loggerConfig.WriteTo.Async(a => a.File(
            path: logPath,
            outputTemplate: options.Logging.OutputTemplate,
            rollingInterval: fileOptions.RollingInterval,
            retainedFileCountLimit: fileOptions.RetainedFileCountLimit,
            fileSizeLimitBytes: fileOptions.FileSizeLimitBytes,
            rollOnFileSizeLimit: fileOptions.RollOnFileSizeLimit,
            restrictedToMinimumLevel: ConvertLogLevel(fileOptions.MinimumLevel),
            flushToDiskInterval: fileOptions.FlushInterval),
            bufferSize: fileOptions.BufferSize);
    }

    /// <summary>
    /// Resolves environment variable placeholders in a file path template.
    /// </summary>
    /// <param name="pathTemplate">The path template with placeholders like {LocalAppData}, {AppData}, {UserProfile}, {Temp}, {ServiceName}.</param>
    /// <param name="serviceName">The service name to substitute for {ServiceName} placeholder.</param>
    /// <returns>The resolved file path with all placeholders replaced and directory ensured to exist.</returns>
    /// <remarks>
    /// Supported placeholders:
    /// - {LocalAppData}: User's local application data directory
    /// - {AppData}: User's application data directory
    /// - {UserProfile}: User's profile directory
    /// - {Temp}: Temporary directory
    /// - {ServiceName}: Replaced with the provided service name
    /// 
    /// Automatically creates the target directory if it does not already exist.
    /// </remarks>
    private static string ResolvePath(string pathTemplate, string serviceName)
    {
        var path = pathTemplate;

        // Replace placeholders
        path = path.Replace("{LocalAppData}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        path = path.Replace("{AppData}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        path = path.Replace("{UserProfile}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        path = path.Replace("{Temp}", Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar));
        path = path.Replace("{ServiceName}", serviceName);

        // Ensure directory exists
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return path;
    }

    /// <summary>
    /// Converts Microsoft.Extensions.Logging LogLevel to Serilog LogEventLevel.
    /// </summary>
    /// <param name="logLevel">The Microsoft.Extensions.Logging log level.</param>
    /// <returns>The corresponding Serilog log event level.</returns>
    /// <remarks>
    /// Maps all Microsoft log levels to their Serilog equivalents:
    /// - Trace → Verbose
    /// - Debug → Debug
    /// - Information → Information
    /// - Warning → Warning
    /// - Error → Error
    /// - Critical → Fatal
    /// - None → Fatal
    /// </remarks>
    private static LogEventLevel ConvertLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }
}
