using ActivityPaint.Application.Abstractions.Repository.Models;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.Repository;

public interface IRepositoryService
{
    public Result CreateRepository(string path, AuthorModel author, ICollection<CommitModel> commits, Progress? progressCallback = null);
    public Task<Result<Stream>> CreateRepositoryZipAsync(string path, AuthorModel author, ICollection<CommitModel> commits,
        Progress? progressCallback = null, CancellationToken cancellationToken = default);
}
