using ActivityPaint.Integration.Database.SeedData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Integration.Database.SeedData;

internal interface IDatabaseSeedService
{
    Task SeedAsync(CancellationToken cancellationToken);
}

internal class DatabaseSeedService : IDatabaseSeedService
{
    private readonly IDbContextFactory<ActivityPaintContext> _dbContextFactory;
    private readonly ILogger _logger;

    public DatabaseSeedService(IDbContextFactory<ActivityPaintContext> dbContextFactory, ILogger<DatabaseSeedService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        _logger.LogInformation("Creating database and running migrations..");
        await context.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Seeding database..");
        await context.SeedPresetsAsync(_logger, cancellationToken);
        await context.SeedRepositoryConfigsAsync(_logger, cancellationToken);

        _logger.LogInformation("Saving changes..");
        await context.BulkSaveChangesAsync(cancellationToken);

        _logger.LogInformation("Closing the database connection..");
        await context.Database.CloseConnectionAsync();

    }
}
