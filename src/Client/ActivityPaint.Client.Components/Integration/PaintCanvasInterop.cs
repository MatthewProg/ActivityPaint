﻿using ActivityPaint.Client.Components.Models;
using ActivityPaint.Core.Enums;
using Microsoft.JSInterop;

namespace ActivityPaint.Client.Components.Integration;

public class PaintCanvasInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public PaintCanvasInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ActivityPaint.Client.Components/UI/Editor/Paint/PaintCanvasComponent.razor.js").AsTask());
    }

    public async ValueTask Init()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("init");
    }

    public async ValueTask UpdateSettings(PaintCanvasModel settings)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("updateSettings", settings);
    }

    public async ValueTask<List<IntensityEnum>> SerializeCanvas()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<List<IntensityEnum>>("serializeCanvas");
    }

    public async ValueTask ResetCanvas()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("resetCanvas");
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
