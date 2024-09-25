using ActivityPaint.Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.Services;

internal class DatabaseConfigService(IDbContextFactory<ActivityPaintContext> dbContextFactory) : IDatabaseConfigService
{
    private readonly IDbContextFactory<ActivityPaintContext> _dbContextFactory = dbContextFactory;

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
        await context.Database.CloseConnectionAsync();
    }

    public string GetDatabasePath()
        => GlobalConfig.DB_FILE_PATH;
}
