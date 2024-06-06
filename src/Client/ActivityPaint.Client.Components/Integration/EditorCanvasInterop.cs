using ActivityPaint.Client.Components.Models;
using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Integration;

public class EditorCanvasInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public EditorCanvasInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/UI/Editor/EditorCanvasComponent.razor.js").AsTask());
    }

    public async ValueTask Init()
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("init");
    }

    public async ValueTask UpdateSettings(EditorSettingsModel settings)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("updateSettings", settings);
    }

    public async ValueTask Destroy()
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("destroy");
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
