using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.Services;

public interface IFileSystemInteraction
{
    public Task<Result> PromptFileSaveAsync(string fileName, Stream data);
}
