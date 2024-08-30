using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Client.Components;
using ActivityPaint.Client.Mobile.Shared.Interactions;
using ActivityPaint.Integration.Database;
using ActivityPaint.Integration.FileSystem;
using ActivityPaint.Integration.Repository;
using CommunityToolkit.Maui.Storage;

namespace ActivityPaint.Client.Mobile.Shared;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddClientComponents();
        services.AddDatabaseIntegration();
        services.AddFileSystemIntegration();
        services.AddRepositoryIntegration();
        services.AddMobile();
    }

    private static void AddMobile(this IServiceCollection services)
    {
        services.AddSingleton<IFileSaver>(FileSaver.Default);
        services.AddSingleton<IFilePicker>(FilePicker.Default);

        services.AddScoped<IFileSystemInteraction, FileSystemInteraction>();
    }
}
