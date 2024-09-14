using ActivityPaint.Client.Web.E2ETests.Setup;
using Microsoft.Playwright;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class ConfigurationTests(WebApplicationFixture app, PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;
    private readonly WebApplicationFixture _app = app;

    [Theory]
    [ClassData(typeof(AllBrowsersData))]
    public async Task ConfigurationPage_ShouldSaveAndLoadData(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/configuration");

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert - load empty
            (await page.Locator("h1").TextContentAsync()).Should().Be("Configuration");
            (await page.GetByLabel("Author Full Name").InputValueAsync()).Should().BeEmpty();
            (await page.GetByLabel("Author Email").InputValueAsync()).Should().BeEmpty();
            (await page.GetByLabel("Message Format").InputValueAsync()).Should().BeEmpty();

            // Input data
            await page.GetByLabel("Author Full Name").FillAsync("Test");
            await page.GetByLabel("Author Email").FillAsync("test@example.com");
            await page.GetByLabel("Message Format").FillAsync("Format {name} {current_total}");
            await page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();

            // Wait for DB sync and textarea refresh
            await Task.Delay(2000);
            (await page.Locator(".message-preview__textarea textarea").InputValueAsync()).Should().Be("Format Example 1\nFormat Example 2\nFormat Example 3\nFormat Example 4\nFormat Example 5\nFormat Example 6\nFormat Example 7\nFormat Example 8\nFormat Example 9\nFormat Example 10");

            await page.ReloadAsync(new()
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // Assert - data loaded
            (await page.GetByLabel("Author Full Name").InputValueAsync()).Should().Be("Test");
            (await page.GetByLabel("Author Email").InputValueAsync()).Should().Be("test@example.com");
            (await page.GetByLabel("Message Format").InputValueAsync()).Should().Be("Format {name} {current_total}");
            (await page.Locator(".message-preview__textarea textarea").InputValueAsync()).Should().Be("Format Example 1\nFormat Example 2\nFormat Example 3\nFormat Example 4\nFormat Example 5\nFormat Example 6\nFormat Example 7\nFormat Example 8\nFormat Example 9\nFormat Example 10");
        });
    }
}
