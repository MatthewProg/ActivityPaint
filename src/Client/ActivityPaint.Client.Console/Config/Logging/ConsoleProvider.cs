using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ActivityPaint.Client.Console.Config.Logging;

[ProviderAlias("Console")]
internal sealed class ConsoleProvider(LoggerConsoleConfigModel loggerConfig) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly LoggerConsoleConfigModel _loggerConfig = loggerConfig;

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name, _loggerConfig));

    public void Dispose()
    {
        _loggers.Clear();
    }
}
