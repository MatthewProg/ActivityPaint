using ActivityPaint.Client.Components;
using ActivityPaint.Client.Mobile.Shared;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddApplication();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.ValidateComponentsDI();

            return builder.Build();
        }
    }
}
