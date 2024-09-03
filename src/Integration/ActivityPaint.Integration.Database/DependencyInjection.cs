using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Integration.Database.Repositories;
using ActivityPaint.Integration.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Integration.Database;

public static class DependencyInjection
{
    public static void AddDatabaseIntegration(this IServiceCollection services)
    {
        services.AddEntityFramework();
        services.AddRepositories();

        services.AddScoped<IDatabaseConfigService, DatabaseConfigService>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPresetRepository, PresetRepository>();
        services.AddScoped<IRepositoryConfigRepository, RepositoryConfigRepository>();
    }

    private static void AddEntityFramework(this IServiceCollection services)
    {
        services.AddDbContextFactory<ActivityPaintContext>(options =>
        {
            options.UseSqlite($"Filename={GlobalConfig.DB_FILE_PATH}");
#if DEBUG
            options.EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
#endif
        });
    }
}
