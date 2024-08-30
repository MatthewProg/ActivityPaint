using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ActivityPaint.Application.BusinessLogic.Preset.Converters;

internal class CanvasDataConverter : JsonConverter<List<IntensityEnum>>
{
    public override List<IntensityEnum>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => CanvasDataHelper.ConvertToList(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, List<IntensityEnum> value, JsonSerializerOptions options)
        => writer.WriteStringValue(CanvasDataHelper.ConvertToString(value));
}
