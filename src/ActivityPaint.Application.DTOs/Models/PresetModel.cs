using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.DTOs.Models;

public class PresetModel
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public bool IsDarkModeDefault { get; set; }

    public IEnumerable<IntensityEnum> CanvasData { get; set; } = [];
}
