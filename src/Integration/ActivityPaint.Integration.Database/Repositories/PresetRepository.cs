using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.Repositories;

internal class PresetRepository(ActivityPaintContext context) : GenericRepository<Preset>(context), IPresetRepository
{
    public async ValueTask<List<Preset>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page - 1;

        var result = await _context.Presets
                                   .AsNoTracking()
                                   .OrderBy(p => p.Id)
                                   .Skip(normalizedPage * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);

        return result;
    }
}
