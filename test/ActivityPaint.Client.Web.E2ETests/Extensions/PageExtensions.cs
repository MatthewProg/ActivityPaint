using Microsoft.Playwright;
using System.Numerics;

namespace ActivityPaint.Client.Web.E2ETests.Extensions;

public static class PageExtensions
{
    public static async Task DragAndDropStepsAsync(this IPage page, string source, string target, int steps = 0)
    {
        var sourceCentre = await page.Locator(source).PositionAsync();
        var targetCentre = await page.Locator(target).PositionAsync();

        ArgumentNullException.ThrowIfNull(sourceCentre);
        ArgumentNullException.ThrowIfNull(targetCentre);

        await page.Mouse.MoveAsync(sourceCentre.Value.X, sourceCentre.Value.Y);
        await page.Mouse.DownAsync();

        if (steps > 0)
        {
            var pointsDiff = targetCentre.Value - sourceCentre.Value;
            var currentPos = sourceCentre;
            var step = new Vector2(pointsDiff.X / (steps + 1), pointsDiff.Y / (steps + 1));
            for (int i = 0; i < steps; i++)
            {
                currentPos += step;
                await page.Mouse.MoveAsync(currentPos.Value.X, currentPos.Value.Y);
            }
        }

        await page.Mouse.MoveAsync(targetCentre.Value.X, targetCentre.Value.Y);
        await page.Mouse.UpAsync();
    }

    public static async Task<Vector2?> PositionAsync(this ILocator locator)
    {
        var box = await locator.BoundingBoxAsync();

        if (box is null)
        {
            return null;
        }

        return new Vector2(box.X + (box.Width / 2f), box.Y + (box.Height / 2f));
    }
}
