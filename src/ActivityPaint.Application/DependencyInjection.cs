using ActivityPaint.Application.Shared.Mediator.Pipelines;
using ActivityPaint.Core;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddCore();

        services.AddValidation();
        services.AddCQRS();
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
