using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.Abstractions.Repository.Models;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Integration.Repository.Services;

internal class RepositoryService : IRepositoryService
{
    public Result CreateRepository(string path, AuthorModel author, ICollection<CommitModel> commits, Progress? progressCallback = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Stream>> CreateRepositoryZipAsync(string path, AuthorModel author, ICollection<CommitModel> commits, Progress? progressCallback = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
