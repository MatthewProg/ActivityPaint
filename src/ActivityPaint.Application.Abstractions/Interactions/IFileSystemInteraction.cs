using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.Abstractions.Interactions;

public interface IFileSystemInteraction
{
    public Task<Result> PromptFileSaveAsync(string fileName, Stream data, CancellationToken cancellationToken = default);
}
