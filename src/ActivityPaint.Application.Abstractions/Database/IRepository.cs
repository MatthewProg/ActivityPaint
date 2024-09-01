using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.Abstractions.Database;

public interface IRepository<T> where T : BaseEntity
{
    ValueTask<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    ValueTask InsertAsync(T model, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(T model, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
}
