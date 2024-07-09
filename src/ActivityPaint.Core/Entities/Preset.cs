using ActivityPaint.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ActivityPaint.Core.Entities;

public sealed class Preset : BaseEntity
{
    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public bool IsDarkModeDefault { get; set; }

    public IEnumerable<IntensityEnum> CanvasData { get; set; } = [];

    [NotMapped]
    [JsonIgnore]
    public string CanvasDataString => new(CanvasData.Select(x => (char)(x + '0')).ToArray());

    public void SetCanvasDataFromString(string data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var array = data.Select(x => (IntensityEnum)(x - '0')).ToArray();
        CanvasData = array;
    }
}
