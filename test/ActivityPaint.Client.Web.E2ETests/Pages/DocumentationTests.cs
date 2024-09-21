using ActivityPaint.Client.Web.E2ETests.Setup;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class DocumentationTests(PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;

    [Theory]
    [ClassData(typeof(AllBrowsersData))]
    public async Task DocumentationPage_ShouldLoad(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/documentation");

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert
            (await page.Locator("h1").TextContentAsync()).Should().Be("Documentation");
        });
    }
}
