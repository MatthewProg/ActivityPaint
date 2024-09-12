using ActivityPaint.Client.Web.E2ETests.Setup;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class GalleryTests(WebApplicationFixture app, PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;
    private readonly WebApplicationFixture _app = app;

    [Theory]
    [InlineData(BrowserEnum.Chromium)]
    [InlineData(BrowserEnum.Firefox)]
    [InlineData(BrowserEnum.Webkit)]
    public async Task GalleryPage_ShouldLoad(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/gallery");

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert
            await page.Locator("h1:has-text(\"Gallery\")").IsVisibleAsync();
        });
    }
}
