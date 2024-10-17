using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ActivityPaint.Application.BusinessLogic.Image.Services;

internal interface IPreviewImageService
{
    Task<MemoryStream> GeneratePreviewAsync(PresetModel preset, bool? darkModeOverwrite = null, CancellationToken cancellationToken = default);
}

internal class PreviewImageService : IPreviewImageService
{
    public async Task<MemoryStream> GeneratePreviewAsync(PresetModel preset, bool? darkModeOverwrite = null, CancellationToken cancellationToken = default)
    {
        var canvas = preset.CanvasData;
        var weekdayOffset = (int)preset.StartDate.DayOfWeek;
        var darkMode = darkModeOverwrite ?? preset.IsDarkModeDefault;

        using var image = new Image<Rgba32>(705, 94);
        image.ProcessPixelRows(pixels => GenerateImage(pixels, canvas, weekdayOffset, darkMode));

        var memoryStream = new MemoryStream(4096);
        await image.SaveAsPngAsync(memoryStream, cancellationToken);

        return memoryStream;
    }

    private static void GenerateImage(PixelAccessor<Rgba32> pixels, List<IntensityEnum> canvas, int weekdayOffset, bool darkMode)
    {
        for (var y = 0; y < pixels.Height; y++)
        {
            var row = pixels.GetRowSpan(y);
            for (var x = 0; x < row.Length; x++)
            {
                ref var pixel = ref row[x];

                var xMod = x % 13;
                var yMod = y % 13;
                if (x >= row.Length - 3 // rightmost spacing
                    || y >= pixels.Height - 3 // bottom spacing
                    || xMod <= 2 // cell left spacing
                    || yMod <= 2) // cell top spacing
                {
                    pixel = GetBackgroundPixel(darkMode);
                    continue;
                }

                if ((yMod is 3 or 12) && (xMod is 3 or 12)) // cell corner radius
                {
                    pixel = GetBackgroundPixel(darkMode);
                    continue;
                }

                var xPos = x / 13;
                var yPos = y / 13;
                var index = (xPos * 7) + yPos - weekdayOffset;

                if (index < 0 || index > canvas.Count - 1)
                {
                    pixel = GetBackgroundPixel(darkMode);
                }
                else
                {
                    var intensity = canvas[index];
                    pixel = GetIntensityPixel(intensity, darkMode);
                }
            }
        }
    }

    private static Rgba32 GetIntensityPixel(IntensityEnum intensity, bool darkMode) => intensity switch
    {
        IntensityEnum.Level0 when darkMode => new Rgba32(22, 27, 34),
        IntensityEnum.Level1 when darkMode => new Rgba32(14, 68, 41),
        IntensityEnum.Level2 when darkMode => new Rgba32(0, 109, 50),
        IntensityEnum.Level3 when darkMode => new Rgba32(38, 166, 65),
        IntensityEnum.Level4 when darkMode => new Rgba32(57, 211, 83),
        IntensityEnum.Level0 => new Rgba32(235, 237, 240),
        IntensityEnum.Level1 => new Rgba32(155, 233, 168),
        IntensityEnum.Level2 => new Rgba32(64, 196, 99),
        IntensityEnum.Level3 => new Rgba32(48, 161, 78),
        IntensityEnum.Level4 => new Rgba32(33, 110, 57),
        _ => throw new NotImplementedException(),
    };

    private static Rgba32 GetBackgroundPixel(bool darkMode) => darkMode
        ? new Rgba32(13, 17, 23)
        : new Rgba32(255, 255, 255);

}
