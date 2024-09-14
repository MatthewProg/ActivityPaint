using ActivityPaint.Client.Web.E2ETests.Setup;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class GalleryTests(WebApplicationFixture app, PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;
    private readonly WebApplicationFixture _app = app;

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
            (await page.Locator("h1").TextContentAsync()).Should().Be("Gallery");
        });
    }
}
