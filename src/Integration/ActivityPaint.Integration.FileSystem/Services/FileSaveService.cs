using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Integration.FileSystem.Services;

internal class FileSaveService : IFileSaveService
{
    public async Task<Result> SaveFileAsync(string filePath, Stream data, CancellationToken cancellationToken)
    {
        using var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write);

        await data.CopyToAsync(fileStream, cancellationToken);
        await data.FlushAsync(cancellationToken);
        await fileStream.FlushAsync(cancellationToken);

        return Result.Success();
    }
}
