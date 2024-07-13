using ActivityPaint.Application.Abstractions.Interactions;
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

        services.AddValidation();
        services.AddCQRS();
    }

    public static void ValidateBusinessLogicDI(this IServiceCollection services)
    {
        services.ThrowIfNotRegistered<IFileSystemInteraction>();
    }

    private static void AddCQRS(this IServiceCollection services)
    {
        services.AddMediator(x =>
        {
            x.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipeline<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), ServiceLifetime.Transient, includeInternalTypes: true);
    }
}
