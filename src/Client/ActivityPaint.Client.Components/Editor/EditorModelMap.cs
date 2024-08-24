using ActivityPaint.Application.DTOs.Preset;

namespace ActivityPaint.Client.Components.Editor;

public static class EditorModelMap
{
    public static PresetModel ToPresetModel(this EditorModel model) => new(
        Name: model.Name ?? string.Empty,
        IsDarkModeDefault: model.IsDarkModeDefault ?? false,
        StartDate: model.StartDate ?? DateTime.MinValue,
        CanvasData: model.CanvasData ?? []
    );

    public static void MapFromPresetModel(PresetModel presetModel, EditorModel editorModel)
    {
        editorModel.IsDarkModeDefault = presetModel.IsDarkModeDefault;
        editorModel.CanvasData = presetModel.CanvasData;
        editorModel.StartDate = presetModel.StartDate;
        editorModel.Name = presetModel.Name;
    }
}
