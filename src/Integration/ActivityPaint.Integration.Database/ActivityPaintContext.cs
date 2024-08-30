using ActivityPaint.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database;

public class ActivityPaintContext : DbContext
{
    public DbSet<Preset> Presets { get; set; }
    public DbSet<RepositoryConfig> RepositoryConfigs { get; set; }
}
