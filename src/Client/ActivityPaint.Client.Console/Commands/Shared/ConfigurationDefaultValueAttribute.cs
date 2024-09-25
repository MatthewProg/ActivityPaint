using ActivityPaint.Client.Console.Config;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Shared;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationDefaultValueAttribute<T>(string configKey) : DefaultValueAttribute(GetValue(configKey))
{
    private static T? GetValue(string configKey)
    {
        var path = $"Config:{configKey}";
        var value = GlobalConfig.Instance.GetSection(path).Get<T>();

        return value;
    }
}
