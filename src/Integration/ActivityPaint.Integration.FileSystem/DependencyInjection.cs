using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Integration.FileSystem.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Integration.FileSystem;

public static class DependencyInjection
{
    public static void AddFileSystemIntegration(this IServiceCollection services)
    {
        services.AddScoped<IFileSaveService, FileSaveService>();
        services.AddScoped<IFileLoadService, FileLoadService>();
    }
}
