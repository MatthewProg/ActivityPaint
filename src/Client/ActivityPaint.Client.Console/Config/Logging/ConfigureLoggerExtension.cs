using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Console.Config.Logging;

internal static class ConfigureLoggerExtension
{
    public static void ConfigureLogger(this ILoggingBuilder builder, IConfiguration configuration)
    {
        builder.ClearProviders();
        builder.AddConsole(configuration);
        builder.SetMinimumLevel(LogLevel.Trace);
    }

    private static void AddConsole(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var loggerConfig = configuration.GetRequiredSection("Logging:Console").Get<LoggerConfigModel>()!;
        var provider = new ConsoleProvider(loggerConfig);

        builder.AddProvider(provider);
    }
}
