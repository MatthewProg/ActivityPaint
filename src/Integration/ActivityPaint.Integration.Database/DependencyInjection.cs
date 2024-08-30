using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Integration.Database;

public static class DependencyInjection
{
    public static void AddDatabaseIntegration(this IServiceCollection services)
    {
        services.AddEntityFramework();
    }

    private static void AddEntityFramework(this IServiceCollection services)
    {
        services.AddDbContext<ActivityPaintContext>(options =>
        {
#if DEBUG
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
#endif
            options.UseSqlite("Data Source=data/db.sqlite");
        });
    }
}
