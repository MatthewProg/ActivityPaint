using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Core.Entities;

namespace ActivityPaint.Integration.Database.Repositories;

internal class PresetRepository(ActivityPaintContext context) : GenericRepository<Preset>(context), IPresetRepository
{
}
