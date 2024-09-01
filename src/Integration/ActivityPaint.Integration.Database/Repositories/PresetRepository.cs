using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Core.Entities;

namespace ActivityPaint.Integration.Database.Repositories;

internal class PresetRepository : GenericRepository<Preset>, IPresetRepository
{
    public PresetRepository(ActivityPaintContext context) : base(context)
    {
    }
}
