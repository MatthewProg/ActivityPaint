using ActivityPaint.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Integration.Database.SeedData.Data;

internal static class RepositoryConfigsData
{
    public static async Task SeedRepositoryConfigsAsync(this ActivityPaintContext context, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Inserting Repository Configs..");

        var data = GetRepositoryConfigs();
        await context.RepositoryConfigs.BulkMergeAsync(data, cancellationToken);
    }

    public static List<RepositoryConfig> GetRepositoryConfigs() =>
    [
        new()
        {
            Id = 1,
            AuthorEmail = "email@example.com",
            AuthorFullName = "Activity Paint",
            MessageFormat = "ActivityPaint - '{name}' - (Commit {current_total}/{total_count})"
        }
    ];
}
