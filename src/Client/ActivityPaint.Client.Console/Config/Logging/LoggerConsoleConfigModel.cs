using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Console.Config.Logging;

internal class LoggerConsoleConfigModel
{
    public bool Enabled { get; init; } = true;
    public LogLevel MinimumLogLevel { get; init; } = LogLevel.Warning;
}
