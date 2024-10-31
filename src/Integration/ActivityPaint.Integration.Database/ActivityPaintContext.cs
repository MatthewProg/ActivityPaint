using ActivityPaint.Core.Entities;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using ActivityPaint.Integration.Database.Comparers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActivityPaint.Integration.Database;

public sealed class ActivityPaintContext(DbContextOptions<ActivityPaintContext> options) : DbContext(options)
{
    public DbSet<Preset> Presets { get; set; }
    public DbSet<RepositoryConfig> RepositoryConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preset>()
            .Property(p => p.CanvasData)
            .HasConversion(
                d => CanvasDataHelper.ConvertToString(d),
                d => CanvasDataHelper.ConvertToList(d),
                new CollectionValueComparer<IntensityEnum>());

        if (Database.IsSqlite())
        {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                               || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }
}
