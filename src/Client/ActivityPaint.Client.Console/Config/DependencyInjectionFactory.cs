using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic;
using ActivityPaint.Client.Console.Config.DependencyInjection;
using ActivityPaint.Client.Console.Interactions;
using ActivityPaint.Integration.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Config;

internal static class DependencyInjectionFactory
{
    public static ITypeRegistrar GetTypeRegistrar() => new TypeRegistrar(GetServices());

    public static IServiceCollection GetServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddConsole());

        services.AddApplication();

        services.ValidateBusinessLogicDI();

        return services;
    }

    private static void AddApplication(this IServiceCollection services)
    {
        services.AddBusinessLogic();
        services.AddFileSystemIntegration();

        services.AddScoped<IFileSystemInteraction, FileSystemInteraction>();
    }
}
