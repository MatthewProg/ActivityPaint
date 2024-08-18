using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.FileSystem;

public interface IFileLoadService
{
    public Result<Stream> GetFileStream(string filePath);
    public Task<Result<string>> GetFileTextAsync(string filePath, CancellationToken cancellationToken = default);
}
