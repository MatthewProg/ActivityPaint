using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPaint.Application.BusinessLogic.Preset.Converters;

internal class CanvasDataConverter : JsonConverter<IEnumerable<IntensityEnum>>
{
    public override IEnumerable<IntensityEnum>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => CanvasDataHelper.ConvertToList(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, IEnumerable<IntensityEnum> value, JsonSerializerOptions options)
        => writer.WriteStringValue(CanvasDataHelper.ConvertToString(value));
}
