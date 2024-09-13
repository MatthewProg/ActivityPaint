using ActivityPaint.Core.Entities;
using ActivityPaint.Integration.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.IntegrationTests.Repositories;

public sealed class RepositoryConfigRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;
    private readonly ActivityPaintContext _context;

    public RepositoryConfigRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _context = fixture.GetContext();

        CleanTable(_context);
    }

    [Fact]
    public async Task GetFirstAsync_WhenNoEntries_ShouldReturnNull()
    {
        // Arrange
        var repo = new RepositoryConfigRepository(_context);

        // Act
        var result = await repo.GetFirstAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFirstAsync_WhenMultipleEntries_ShouldReturnFirst()
    {
        // Arrange
        var data = GetDummyData();
        await InsertData(data);

        var repo = new RepositoryConfigRepository(_context);

        // Act
        var result = await repo.GetFirstAsync();

        // Assert
        result.Should().BeEquivalentTo(data[0]);
    }

    [Fact]
    public async Task UpsertFirstAsync_WhenTableIsEmpty_ShouldCreate()
    {
        // Arrange
        var data = GetDummyData()[0];

        var repo = new RepositoryConfigRepository(_context);

        // Act
        await repo.UpsertFirstAsync(data);

        // Assert
        var results = _context.RepositoryConfigs.AsNoTracking()
                                                .ToList();
        results.Should().HaveCount(1);
        results[0].Should().BeEquivalentTo(data);
    }

    [Fact]
    public async Task UpsertFirstAsync_WhenTableHasData_ShouldUpdateFirstEntry()
    {
        // Arrange
        var data = GetDummyData();
        await InsertData(data);

        var update = new RepositoryConfig()
        {
            Id = 2,
            AuthorEmail = "dummy@example.com",
            AuthorFullName = "Jane Smith",
            MessageFormat = null
        };
        var expected = new List<RepositoryConfig>()
        {
            new()
            {
                Id = 1,
                AuthorEmail = update.AuthorEmail,
                AuthorFullName = update.AuthorFullName,
                MessageFormat = update.MessageFormat,
            },
            data[1]
        };

        var repo = new RepositoryConfigRepository(_context);

        // Act
        await repo.UpsertFirstAsync(update);

        // Assert
        var results = _context.RepositoryConfigs.AsNoTracking()
                                                .ToList();
        results.Should().HaveCount(2);
        results.Should().BeEquivalentTo(expected);
    }

    private static void CleanTable(ActivityPaintContext context)
    {
        var tableName = context.RepositoryConfigs.EntityType.GetTableName();
        context.RepositoryConfigs.ExecuteDelete();
        context.Database.ExecuteSql($"DELETE FROM `sqlite_sequence` WHERE `name` = {tableName};");
    }

    private async Task InsertData(IEnumerable<RepositoryConfig> data)
    {
        using var ctx = _fixture.GetContext();
        await ctx.RepositoryConfigs.AddRangeAsync(data);
        await ctx.SaveChangesAsync();
    }

    private static List<RepositoryConfig> GetDummyData() =>
    [
        new()
        {
            Id = 1,
            AuthorEmail = "test@example.com",
            AuthorFullName = "John Doe",
            MessageFormat = "Test {name}"
        },
        new()
        {
            Id = 2,
            AuthorEmail = "dummy@example.com",
            AuthorFullName = "Jane Smith",
            MessageFormat = null
        }
    ];

    public void Dispose()
    {
        _context.Dispose();
    }
}
