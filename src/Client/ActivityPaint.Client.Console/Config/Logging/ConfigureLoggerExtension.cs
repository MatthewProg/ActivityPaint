using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace ActivityPaint.Client.Console.Config.Logging;

internal static class ConfigureLoggerExtension
{
    public static void ConfigureLogger(this ILoggingBuilder builder, IConfiguration configuration)
    {
        builder.ClearProviders();
        builder.AddConsole(configuration);
        builder.AddFile(configuration);

        builder.SetMinimumLevel(LogLevel.Trace);
    }

    private static void AddConsole(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var loggerConfig = configuration.GetRequiredSection("Logging:Console").Get<LoggerConsoleConfigModel>()!;

        if (!loggerConfig.Enabled)
        {
            return;
        }

        var provider = new ConsoleProvider(loggerConfig);

        builder.AddProvider(provider);
    }

    private static void AddFile(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var loggerConfig = configuration.GetRequiredSection("Logging:File").Get<LoggerFileConfigModel>()!;

        if (!loggerConfig.Enabled)
        {
            return;
        }

        var path = Path.IsPathFullyQualified(loggerConfig.Path)
            ? loggerConfig.Path
            : GetAssemblyRelativePath(loggerConfig.Path);

        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(
                path,
                restrictedToMinimumLevel: (LogEventLevel)(int)loggerConfig.MinimumLogLevel,
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.AddSerilog(logger, true);
    }

    private static string GetAssemblyRelativePath(string relativePath)
    {
        var assemblyPath = Assembly.GetEntryAssembly()?.Location;
        var assemblyDir = Path.GetDirectoryName(assemblyPath) ?? Directory.GetCurrentDirectory();

        return Path.Combine(assemblyDir, relativePath);
    }
}
