using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.Repositories;

internal abstract class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ActivityPaintContext _context;

    protected GenericRepository(ActivityPaintContext context)
    {
        _context = context;
    }

    public async ValueTask<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var data = await _context.Set<T>()
                                 .ToListAsync(cancellationToken);

        return data;
    }

    public async ValueTask<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _context.Set<T>()
                                 .FirstOrDefaultAsync(cancellationToken);

        return item;
    }

    public async ValueTask InsertAsync(T model, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>()
                      .AddAsync(model, cancellationToken);

        await SaveChangesAsync(cancellationToken);
    }

    public async ValueTask UpdateAsync(T model, CancellationToken cancellationToken = default)
    {
        _context.Set<T>()
                .Update(model);

        await SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _context.Set<T>()
                                 .FindAsync(id, cancellationToken)
                                 ?? throw new KeyNotFoundException($"Item with ID: {id} was not found");

        _context.Set<T>()
                .Remove(item);

        await SaveChangesAsync(cancellationToken);
    }

    private async ValueTask SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
