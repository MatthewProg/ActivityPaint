using ActivityPaint.Core.Entities;
using ActivityPaint.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Integration.Database.SeedData.Data;

internal static class PresetsData
{
    public static async Task SeedPresetsAsync(this ActivityPaintContext context, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Inserting Presets..");

        var data = GetPresets();
        await context.Presets.BulkMergeAsync(data, cancellationToken);
    }

    public static List<Preset> GetPresets() =>
    [
        new()
        {
            Id = 1,
            Name = "Test",
            IsDarkModeDefault = false,
            StartDate = new DateTime(2024, 1, 1),
            CanvasData = CanvasDataHelper.ConvertToList("eAFsUEEOACAIstb/3xwKVs44yFRgTrOOSQyi7hdgXoLqTt0iJ1Gc3dchw1FCjHirBJEPnaK2QkvzPSmZ2Kw3VNk6JbufQbNQno9UoT6of2K3AQAA//8="),
            LastUpdated = DateTimeOffset.UtcNow.AddDays(-1).AddHours(-1).AddMinutes(-1).AddSeconds(-1)
        },
        new()
        {
            Id = 2,
            Name = "Rainbow",
            IsDarkModeDefault = true,
            StartDate = new DateTime(2020, 1, 1),
            CanvasData = CanvasDataHelper.ConvertToList("eAFiZmJkYGRiZmGmGs0ANY9Ymtr2D2bzAAAAAP//"),
            LastUpdated = DateTimeOffset.UtcNow
        }
    ];
}
