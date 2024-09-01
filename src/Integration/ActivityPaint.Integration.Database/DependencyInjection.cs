using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Integration.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Integration.Database;

public static class DependencyInjection
{
    public static void AddDatabaseIntegration(this IServiceCollection services)
    {
        services.AddEntityFramework();
        services.AddRepositories();
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
            options.UseSqlite("Data Source=C:/tmp/db.sqlite");
#if DEBUG
            options.EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
#endif
        });
    }
}
