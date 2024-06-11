using ActivityPaint.Client.Components.Models;
using ActivityPaint.Core.Enums;
using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Integration;

public class PaintCanvasInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public PaintCanvasInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/UI/Editor/PaintStage/PaintCanvasComponent.razor.js").AsTask());
    }

    public async ValueTask Init()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("init");
    }

    public async ValueTask UpdateSettings(EditorSettingsModel settings)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("updateSettings", settings);
    }

    public async ValueTask<IntensityEnum[]> SerializeCanvas()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<IntensityEnum[]>("serializeCanvas");
    }

    public async ValueTask Destroy()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("destroy");
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
