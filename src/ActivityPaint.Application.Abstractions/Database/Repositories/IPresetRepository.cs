using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.Abstractions.Database.Repositories;

public interface IPresetRepository : IRepository<Preset>
{
    ValueTask<List<Preset>> GetPageAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    ValueTask<int> GetCount(CancellationToken cancellationToken = default);
}
