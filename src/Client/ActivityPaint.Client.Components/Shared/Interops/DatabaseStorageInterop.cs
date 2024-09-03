
using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Shared.Interops;

public interface IDatabaseStorageInterop : IAsyncDisposable
{
    ValueTask SynchronizeFileWithIndexedDb(string filename, CancellationToken cancellationToken = default);
}

public sealed class DatabaseStorageInterop : IDatabaseStorageInterop
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public DatabaseStorageInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/js/database-storage.js").AsTask());
    }

    public async ValueTask SynchronizeFileWithIndexedDb(string filename, CancellationToken cancellationToken = default)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", cancellationToken, filename);
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
