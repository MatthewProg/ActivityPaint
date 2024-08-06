using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ActivityPaint.Client.Console.Config.Logging;

[ProviderAlias("Console")]
internal sealed class ConsoleProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly LoggerConfigModel _loggerConfig;

    public ConsoleProvider(LoggerConfigModel loggerConfig)
    {
        _loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new ConsoleLogger(name, _loggerConfig));

    public void Dispose()
    {
        _loggers.Clear();
    }
}
