using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Console.Config.Logging;

internal class LoggerConfigModel
{
    public LogLevel MinimumLogLevel { get; init; } = LogLevel.Warning;
}
