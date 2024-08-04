using ActivityPaint.Core.Enums;
using System.IO.Compression;

namespace ActivityPaint.Core.Helpers;

public static class CanvasDataHelper
{
    public static string ConvertToString(IEnumerable<IntensityEnum> canvasData)
    {
        ArgumentNullException.ThrowIfNull(canvasData);

        var bytes = canvasData.Select(x => (byte)x)
                              .ToArray();

        if (bytes.Length == 0)
        {
            return string.Empty;
        }

        using var outputStream = new MemoryStream();
        using var compressionStream = new ZLibStream(outputStream, CompressionLevel.Fastest);

        compressionStream.Write(bytes);
        compressionStream.Flush();

        var compressedBytes = outputStream.ToArray();
        var output = Convert.ToBase64String(compressedBytes);

        return output;
    }

    public static List<IntensityEnum> ConvertToList(string canvasDataString)
    {
        ArgumentNullException.ThrowIfNull(canvasDataString);

        if (canvasDataString == string.Empty)
        {
            return [];
        }

        var bytes = Convert.FromBase64String(canvasDataString);

        using var inputStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();
        using var compressionStream = new ZLibStream(inputStream, CompressionMode.Decompress);

        compressionStream.CopyTo(outputStream);
        compressionStream.Flush();

        var decompressedBytes = outputStream.ToArray();
        var output = decompressedBytes.Select(x => (IntensityEnum)x)
                                      .ToList();

        return output;
    }
}
