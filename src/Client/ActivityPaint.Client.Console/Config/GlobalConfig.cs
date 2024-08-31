using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ActivityPaint.Client.Console.Config;

public static class GlobalConfig
{
    public static IConfiguration Instance { get; } = GetConfiguration();

    private static IConfiguration GetConfiguration()
    {
        var assemblyPath = Assembly.GetEntryAssembly()?.Location;
        var assemblyDir = Path.GetDirectoryName(assemblyPath);

        var builder = new ConfigurationBuilder()
            .SetBasePath(assemblyDir ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false);

        return builder.Build();
    }
}
