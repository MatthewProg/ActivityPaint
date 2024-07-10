using ActivityPaint.Application.Abstractions.Services;
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

    public async Task<Result> PromptFileSaveAsync(string fileName, Stream data)
    {
        await _fileSystemInterop.DownloadFile(fileName, data);

        return Result.Success();
    }
}
