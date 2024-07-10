using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Shared.Interops;

public interface IFileSystemInterop : IAsyncDisposable
{
    ValueTask DownloadFile(string fileName, Stream data);
}

public sealed class FileSystemInterop : IFileSystemInterop
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public FileSystemInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/js/file-system.js").AsTask());
    }

    public async ValueTask DownloadFile(string fileName, Stream data)
    {
        var module = await _moduleTask.Value;

        using var streamRef = new DotNetStreamReference(data);

        await module.InvokeVoidAsync("downloadFile", fileName, streamRef);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}