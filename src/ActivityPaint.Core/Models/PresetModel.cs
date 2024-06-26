using ActivityPaint.Core.Enums;

namespace ActivityPaint.Core.Models;

public record PresetModel(
    string Name,
    DateTime StartDate,
    bool IsDarkModeDefault,
    IEnumerable<IntensityEnum>? CanvasData);
