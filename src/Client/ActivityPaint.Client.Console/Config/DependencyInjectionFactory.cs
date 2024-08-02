﻿using ActivityPaint.Application.BusinessLogic;
using ActivityPaint.Integration.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Config;

internal static class DependencyInjectionFactory
{
    public static ITypeRegistrar GetTypeRegistrar() => new TypeRegistrar(GetServices());
    public static IServiceCollection GetServices()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        return services;
    }

    private static void AddApplication(this IServiceCollection services)
    {
        services.AddBusinessLogic();
        services.AddFileSystemIntegration();
    }
}
