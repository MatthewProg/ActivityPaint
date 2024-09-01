using ActivityPaint.Application.BusinessLogic;
using ActivityPaint.Client.Components.Editor.Paint.Canvas;
using ActivityPaint.Client.Components.Shared.Interops;
using ActivityPaint.Client.Components.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace ActivityPaint.Client.Components;

public static class DependencyInjection
{
    public static void AddClientComponents(this IServiceCollection services)
    {
        services.AddBusinessLogic();

        services.AddMudBlazor();
        services.AddComponents();
    }

    public static void ValidateComponentsDI(this IServiceCollection services)
    {
        services.ValidateBusinessLogicDI();
    }

    private static void AddComponents(this IServiceCollection services)
    {
        services.AddScoped<IPaintCanvasInterop, PaintCanvasInterop>();
        services.AddScoped<IFileSystemInterop, FileSystemInterop>();
        services.AddScoped<IDatabaseStorageInterop, DatabaseStorageInterop>();

        services.AddScoped<IAppInitializationService, AppInitializationService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
    }

    private static void AddMudBlazor(this IServiceCollection services)
    {
        services.AddMudServices(x =>
        {
            x.SnackbarConfiguration.ShowTransitionDuration = 150;
            x.SnackbarConfiguration.HideTransitionDuration = 250;
        });
    }
}
