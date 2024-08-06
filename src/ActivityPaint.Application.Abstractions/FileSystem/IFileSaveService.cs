using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.FileSystem;

public interface IFileSaveService
{
    public Task<Result> SaveFileAsync(string filePath, Stream data, bool overwrite = false, CancellationToken cancellationToken = default);
}
