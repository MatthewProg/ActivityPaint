using ActivityPaint.Core.Entities;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using ActivityPaint.Integration.Database.Comparers;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database;

public sealed class ActivityPaintContext : DbContext
{
    public DbSet<Preset> Presets { get; set; }
    public DbSet<RepositoryConfig> RepositoryConfigs { get; set; }

    public ActivityPaintContext(DbContextOptions<ActivityPaintContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preset>()
            .Property(p => p.CanvasData)
            .HasConversion(
                d => CanvasDataHelper.ConvertToString(d),
                d => CanvasDataHelper.ConvertToList(d),
                new CollectionValueComparer<IntensityEnum>());
    }
}
