using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Console.Config.Logging;

internal class LoggerFileConfigModel
{
    public bool Enabled { get; init; } = true;
    public string Path { get; init; } = "logs/logs.log";
    public LogLevel MinimumLogLevel { get; init; } = LogLevel.Debug;
}
