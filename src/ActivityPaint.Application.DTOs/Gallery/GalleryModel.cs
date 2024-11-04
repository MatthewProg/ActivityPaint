using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.DTOs.Gallery;

public sealed record GalleryModel(
    int Id,
    DateTimeOffset LastUpdated,
    string Name,
    DateTime StartDate,
    bool IsDarkModeDefault,
    List<IntensityEnum> CanvasData)
: PresetModel(Name, StartDate, IsDarkModeDefault, CanvasData);
