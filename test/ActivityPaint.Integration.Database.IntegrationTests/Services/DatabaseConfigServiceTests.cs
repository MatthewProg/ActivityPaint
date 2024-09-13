using ActivityPaint.Integration.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.IntegrationTests.Services;

public class DatabaseConfigServiceTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture = fixture;

    [Fact]
    public async Task EnsureCreatedAsync_ShouldCreateAndMigrateDatabase()
    {
        // Arrange
        var service = new DatabaseConfigService(_fixture.ContextFactory);

        // Act
        await service.EnsureCreatedAsync();

        // Assert
        var context = _fixture.ContextFactory.CreateDbContext();
        var migrationsCount = context.Database.SqlQueryRaw<int>("SELECT COUNT(*) FROM __EFMigrationsHistory")
                                              .ToList()
                                              .FirstOrDefault();
        migrationsCount.Should().BeGreaterThan(0);
    }
}
