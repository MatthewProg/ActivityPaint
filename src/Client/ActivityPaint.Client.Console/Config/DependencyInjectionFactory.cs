using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic;
using ActivityPaint.Client.Console.Config.DependencyInjection;
using ActivityPaint.Client.Console.Config.Logging;
using ActivityPaint.Client.Console.Interactions;
using ActivityPaint.Client.Console.Services;
using ActivityPaint.Integration.FileSystem;
using ActivityPaint.Integration.Repository;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Config;

internal static class DependencyInjectionFactory
{
    public static ITypeRegistrar GetTypeRegistrar() => new TypeRegistrar(GetServices());

    public static IServiceCollection GetServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.ConfigureLogger(GlobalConfig.Instance));

        services.AddApplication();

        services.ValidateBusinessLogicDI();

        return services;
    }

    private static void AddApplication(this IServiceCollection services)
    {
        services.AddBusinessLogic();
        services.AddFileSystemIntegration();
        services.AddRepositoryIntegration();

        services.AddScoped<IFileSystemInteraction, FileSystemInteraction>();
        services.AddScoped<IErrorFeedbackService, ErrorFeedbackService>();
    }
}
