using ActivityPaint.Application.Abstractions.Services;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator.Pipelines;
using ActivityPaint.Core;
using ActivityPaint.Core.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application.BusinessLogic;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddCore();

        services.AddValidation();
        services.AddCQRS();
    }

    public static void ValidateApplicationDI(this IServiceCollection services)
    {
        services.ThrowIfNotRegistered<IFileSystemInteraction>();
    }

    private static void AddCQRS(this IServiceCollection services)
    {
        services.AddMediator();

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipeline<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), ServiceLifetime.Transient, includeInternalTypes: true);
    }
}
