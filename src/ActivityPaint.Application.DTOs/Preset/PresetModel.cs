using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;

namespace ActivityPaint.Application.DTOs.Preset;

public sealed record PresetModel(
    string Name,
    DateTime StartDate,
    bool IsDarkModeDefault,
    List<IntensityEnum> CanvasData)
{
    public string CanvasDataString => CanvasDataHelper.ConvertToString(CanvasData);
}
