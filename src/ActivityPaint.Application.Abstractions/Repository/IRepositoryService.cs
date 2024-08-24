using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.Repository;

public interface IRepositoryService
{
    public Result InitOrPopulateRepository(string path, AuthorModel author, IList<CommitModel> commits, Progress? progressCallback = null);
    public Result<Stream> CreateRepositoryZip(AuthorModel author, IList<CommitModel> commits, Progress? progressCallback = null);
}
