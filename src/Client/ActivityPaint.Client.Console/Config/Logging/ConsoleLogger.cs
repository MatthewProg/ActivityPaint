using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ActivityPaint.Client.Console.Config.Logging;

internal sealed class ConsoleLogger(string name, LoggerConsoleConfigModel config) : ILogger
{
    private readonly LoggerConsoleConfigModel _config = config;
    private readonly string _name = name;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => default!;

    public bool IsEnabled(LogLevel logLevel)
        => logLevel >= _config.MinimumLogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var color = GetLevelColor(logLevel);
        var lvlName = GetLevelName(logLevel);
        var msgColor = GetMessageColor(logLevel);
        var message = formatter(state, exception);

        AnsiConsole.MarkupLine($"[[[{color}]{lvlName}[/]]][{msgColor}] {message.EscapeMarkup()}[/]");

        if (exception is not null)
        {
#if DEBUG
            var exceptionFormat = ExceptionFormats.Default;
#else
            var exceptionFormat = ExceptionFormats.NoStackTrace;
#endif
            AnsiConsole.WriteException(exception, exceptionFormat);
        }
    }

    private static string GetLevelColor(LogLevel level) => level switch
    {
        LogLevel.Trace or LogLevel.Debug => "grey",
        LogLevel.Information => "lime",
        LogLevel.Warning => "yellow",
        LogLevel.Error or LogLevel.Critical => "red",
        _ => "silver",
    };

    private static string GetLevelName(LogLevel level) => level switch
    {
        LogLevel.Trace => "TRACE",
        LogLevel.Debug => "DEBUG",
        LogLevel.Information => "INFO",
        LogLevel.Warning => "WARN",
        LogLevel.Error => "ERROR",
        LogLevel.Critical => "FATAL",
        _ => "LOG"
    };

    private static string GetMessageColor(LogLevel level) => level switch
    {
        LogLevel.Trace or LogLevel.Debug => "grey",
        LogLevel.Critical => "red",
        _ => "silver"
    };
}
