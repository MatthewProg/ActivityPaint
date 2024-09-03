using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.Abstractions.Database.Repositories;

public interface IRepositoryConfigRepository : IRepository<RepositoryConfig>
{
    ValueTask<RepositoryConfig?> GetFirstAsync(CancellationToken cancellationToken = default);
    ValueTask UpsertFirstAsync(RepositoryConfig model, CancellationToken cancellationToken = default);
}
