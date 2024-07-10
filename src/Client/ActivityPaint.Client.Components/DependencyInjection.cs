using ActivityPaint.Application.BusinessLogic;
using ActivityPaint.Client.Components.Editor.Paint.Canvas;
using ActivityPaint.Client.Components.Shared.Interops;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace ActivityPaint.Client.Components;

public static class DependencyInjection
{
    public static void AddClientComponents(this IServiceCollection services)
    {
        services.AddApplication();

        services.AddMudServices();
        services.AddComponents();
    }

    public static void ValidateComponentsDI(this IServiceCollection services)
    {
        services.ValidateApplicationDI();
    }

    private static void AddComponents(this IServiceCollection services)
    {
        services.AddScoped<IPaintCanvasInterop, PaintCanvasInterop>();
        services.AddScoped<IFileSystemInterop, FileSystemInterop>();
    }
}
