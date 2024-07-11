using ActivityPaint.Core.Enums;

namespace ActivityPaint.Core.Helpers;

public static class CanvasDataHelper
{
    public static string ConvertToString(IEnumerable<IntensityEnum> canvasData)
    {
        ArgumentNullException.ThrowIfNull(canvasData);

        var charArray = canvasData.Select(x => (char)(x + '0'))
                                  .ToArray();

        return new string(charArray);
    }

    public static List<IntensityEnum> ConvertToList(string canvasDataString)
    {
        ArgumentNullException.ThrowIfNull(canvasDataString);

        var array = canvasDataString.Select(x => (IntensityEnum)(x - '0'))
                                    .ToList();

        return array;
    }
}
