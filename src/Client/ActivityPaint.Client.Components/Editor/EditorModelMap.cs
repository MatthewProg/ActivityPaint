using ActivityPaint.Application.DTOs.Models;

namespace ActivityPaint.Client.Components.Editor;

public static class EditorModelMap
{
    public static PresetModel ToPresetModel(this EditorModel model) => new(
        Name: model.Name ?? string.Empty,
        IsDarkModeDefault: model.IsDarkModeDefault ?? false,
        StartDate: model.StartDate ?? DateTime.MinValue,
        CanvasData: model.CanvasData ?? []
    );
}
