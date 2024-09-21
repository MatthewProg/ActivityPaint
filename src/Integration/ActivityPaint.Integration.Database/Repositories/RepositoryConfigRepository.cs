using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityPaint.Integration.Database.Repositories;

internal class RepositoryConfigRepository(ActivityPaintContext context) : GenericRepository<RepositoryConfig>(context), IRepositoryConfigRepository
{
    private readonly ActivityPaintContext _context = context;

    public async ValueTask<RepositoryConfig?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        var item = await _context.RepositoryConfigs
                                 .AsNoTracking()
                                 .OrderBy(x => x.Id)
                                 .FirstOrDefaultAsync(cancellationToken);

        return item;
    }

    public async ValueTask UpsertFirstAsync(RepositoryConfig model, CancellationToken cancellationToken = default)
    {
        var dbItem = await GetFirstAsync(cancellationToken);

        if (dbItem is null)
        {
            await InsertAsync(model, cancellationToken);
            return;
        }

        model.Id = dbItem.Id;
        await UpdateAsync(model, cancellationToken);
    }
}
