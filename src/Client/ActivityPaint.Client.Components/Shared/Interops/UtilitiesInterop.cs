using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Shared.Interops;

public interface IUtilitiesInterop : IAsyncDisposable
{
    ValueTask CopyToClipboard(string text, CancellationToken cancellationToken = default);
    ValueTask CopyElementTextToClipboard(string selector, CancellationToken cancellationToken = default);
}

public sealed class UtilitiesInterop(IJSRuntime jsRuntime) : IUtilitiesInterop
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/js/utilities.js").AsTask());

    public async ValueTask CopyToClipboard(string text, CancellationToken cancellationToken)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("copyToClipboard", cancellationToken, text);
    }


    public async ValueTask CopyElementTextToClipboard(string selector, CancellationToken cancellationToken)
    {
        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("copyElementTextToClipboard", cancellationToken, selector);
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