using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Image.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator.Pipelines;
using ActivityPaint.Application.DTOs;
using ActivityPaint.Core;
using ActivityPaint.Core.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application.BusinessLogic;

public static class DependencyInjection
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddCore();
        services.AddDTOs();

        services.AddCQRS();
        services.AddServices();
        services.AddValidation();
    }

    public static void ValidateBusinessLogicDI(this IServiceCollection services)
    {
        services.ThrowIfNotRegistered<IFileSystemInteraction>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICommitsService, CommitsService>();
        services.AddScoped<IPreviewImageService, PreviewImageService>();
    }

    private static void AddCQRS(this IServiceCollection services)
    {
        services.AddMediator(x =>
        {
            x.ServiceLifetime = ServiceLifetime.Transient;
        });

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipeline<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), ServiceLifetime.Transient, includeInternalTypes: true);
    }
}
