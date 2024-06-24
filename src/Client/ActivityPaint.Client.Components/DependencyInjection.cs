using ActivityPaint.Application;
using ActivityPaint.Client.Components.Editor.Paint.Canvas;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace ActivityPaint.Client.Components;

public static class DependencyInjection
{
    public static void AddClientComponents(this IServiceCollection services)
    {
        services.AddApplication();

        services.AddMudServices();
        services.AddIntegration();
    }

    private static void AddIntegration(this IServiceCollection services)
    {
        services.AddScoped<PaintCanvasInterop>();
    }
}
