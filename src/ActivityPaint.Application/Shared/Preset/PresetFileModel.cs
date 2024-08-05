using ActivityPaint.Core.Enums;
using System.Text.Json.Serialization;

namespace ActivityPaint.Application.BusinessLogic.Shared.Preset;

internal sealed record PresetFileModel
{
    public required string Name { get; init; }

    public DateTime StartDate { get; init; }

    public bool IsDarkModeDefault { get; init; }

    [JsonConverter(typeof(CanvasDataConverter))]
    public required IEnumerable<IntensityEnum> CanvasData { get; init; }
}
