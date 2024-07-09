using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Integration.FileSystem.Services;

internal class FileSaveService : IFileSaveService
{
    public Task<Result> SaveFileAsync(string filePath, Stream data)
    {
        throw new NotImplementedException();
    }
}
