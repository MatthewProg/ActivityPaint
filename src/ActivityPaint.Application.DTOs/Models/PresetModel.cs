using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;

namespace ActivityPaint.Application.DTOs.Models;

public sealed record PresetModel(
    string Name,
    DateTime StartDate,
    bool IsDarkModeDefault,
    IEnumerable<IntensityEnum> CanvasData)
{
    public string CanvasDataString => CanvasDataHelper.ConvertToString(CanvasData);
}
