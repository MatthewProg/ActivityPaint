using ActivityPaint.Core.Enums;

namespace ActivityPaint.Core.Entities;

public sealed class Preset : BaseEntity
{
    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public bool IsDarkModeDefault { get; set; }

    public IEnumerable<IntensityEnum> CanvasData { get; set; } = [];
}
