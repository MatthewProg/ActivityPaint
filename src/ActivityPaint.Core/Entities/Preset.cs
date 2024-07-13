using ActivityPaint.Core.Enums;

namespace ActivityPaint.Core.Entities;

public sealed class Preset : BaseEntity
{
    public required string Name { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsDarkModeDefault { get; set; }

    public required IEnumerable<IntensityEnum> CanvasData { get; set; }
}
