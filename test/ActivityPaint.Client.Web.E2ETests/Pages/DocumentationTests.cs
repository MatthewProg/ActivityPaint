using ActivityPaint.Client.Web.E2ETests.Setup;
using Xunit.Abstractions;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class DocumentationTests : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright;
    private readonly WebApplicationFixture _app;

    public DocumentationTests(WebApplicationFixture app, PlaywrightFixture playwright, ITestOutputHelper testOutputHelper)
    {
        _playwright = playwright;
        _playwright.SetOutputHelper(testOutputHelper);
        _app = app;
    }

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
