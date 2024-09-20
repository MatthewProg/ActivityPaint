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

        services.AddTransient<IDatabaseConfigService, DatabaseConfigService>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPresetRepository, PresetRepository>();
        services.AddTransient<IRepositoryConfigRepository, RepositoryConfigRepository>();
    }

    private static void AddEntityFramework(this IServiceCollection services)
    {
        services.AddDbContext<ActivityPaintContext>(options =>
        {
            options.UseSqlite($"Filename={GlobalConfig.DB_FILE_PATH}");
#if DEBUG
            options.EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
#endif
        }, contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);

        services.AddDbContextFactory<ActivityPaintContext>(x =>
        {
            // This should never be called, as AddDbContext already sets options
            throw new NotImplementedException("Database configuration is not set!");
        }, ServiceLifetime.Singleton);
    }
}
