using ActivityPaint.Client.Web.E2ETests.Setup;
using Microsoft.Playwright;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class GalleryTests(PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;

    [Theory]
    [ClassData(typeof(AllBrowsersData))]
    public async Task GalleryPage_ShouldLoad(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/gallery");

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert
            (await GetTextHeader(page).TextContentAsync()).Should().Be("Gallery");
        });
    }

    // Text
    private static ILocator GetTextHeader(IPage page) => page.Locator("h1");
}
