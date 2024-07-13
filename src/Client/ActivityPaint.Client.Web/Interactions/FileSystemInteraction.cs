using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Client.Components.Shared.Interops;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Client.Web.Interactions;

public class FileSystemInteraction : IFileSystemInteraction
{
    private readonly IFileSystemInterop _fileSystemInterop;

    public FileSystemInteraction(IFileSystemInterop fileSystemInterop)
    {
        _fileSystemInterop = fileSystemInterop;
    }

    public async Task<Result> PromptFileSaveAsync(string fileName, Stream data, CancellationToken cancellationToken)
    {
        await _fileSystemInterop.DownloadFile(fileName, data, cancellationToken);

        return Result.Success();
    }
}
