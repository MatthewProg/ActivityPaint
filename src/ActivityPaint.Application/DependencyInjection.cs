using ActivityPaint.Application.Shared.Mediator.Pipelines;
using ActivityPaint.Core;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddCore();

        services.AddMediatorComponents();
    }

    private static void AddMediatorComponents(this IServiceCollection services)
    {
        services.AddMediator();

        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipeline<,>));
    }
}
