using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.IntegrationTests;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly string _dbFile = $"{Guid.NewGuid()}.sqlite";

    private bool _initialized = false;

    public ActivityPaintContextFactory ContextFactory { get; init; }

    public ActivityPaintContext GetContext()
    {
        var context = ContextFactory.CreateDbContext();

        if (!_initialized)
        {
            context.Database.Migrate();
            _initialized = true;
        }

        return context;
    }

    public DatabaseFixture()
    {
        ContextFactory = new ActivityPaintContextFactory(_dbFile);
    }

    public async Task InitializeAsync()
    {
        using var ctx = ContextFactory.CreateDbContext();
        await ctx.Database.EnsureDeletedAsync();
    }

    public Task DisposeAsync()
    {
        using var ctx = ContextFactory.CreateDbContext();
        if (ctx.Database.GetDbConnection() is SqliteConnection sqliteConn)
        {
            SqliteConnection.ClearPool(sqliteConn);
        }

        File.Delete(_dbFile);
        return Task.CompletedTask;
    }
}

public sealed class ActivityPaintContextFactory(string dbFile) : IDbContextFactory<ActivityPaintContext>
{
    private readonly string _dbFile = dbFile;

    public ActivityPaintContext CreateDbContext()
        => new(new DbContextOptionsBuilder<ActivityPaintContext>().EnableDetailedErrors()
                                                                  .EnableSensitiveDataLogging()
                                                                  .UseSqlite($"Filename={_dbFile}")
                                                                  .Options);
}