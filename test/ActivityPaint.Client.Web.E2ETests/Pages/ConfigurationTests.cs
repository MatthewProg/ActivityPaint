using ActivityPaint.Client.Web.E2ETests.Setup;
using Microsoft.Playwright;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class ConfigurationTests(PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;

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
            (await GetTextHeader(page).TextContentAsync()).Should().Be("Configuration");
            (await GetFieldName(page).InputValueAsync()).Should().BeEmpty();
            (await GetFieldEmail(page).InputValueAsync()).Should().BeEmpty();
            (await GetFieldMessage(page).InputValueAsync()).Should().BeEmpty();

            // Input data
            await GetFieldName(page).FillAsync("Test");
            await GetFieldEmail(page).FillAsync("test@example.com");
            await GetFieldMessage(page).FillAsync("Format {name} {current_total}");
            await GetButtonSave(page).ClickAsync();

            // Wait for DB sync and textarea refresh
            await Task.Delay(2000);
            (await GetTextMessagePreview(page).InputValueAsync()).Should().Be("Format Example 1\nFormat Example 2\nFormat Example 3\nFormat Example 4\nFormat Example 5\nFormat Example 6\nFormat Example 7\nFormat Example 8\nFormat Example 9\nFormat Example 10");

            await page.ReloadAsync(new()
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // Assert - data loaded
            (await GetFieldName(page).InputValueAsync()).Should().Be("Test");
            (await GetFieldEmail(page).InputValueAsync()).Should().Be("test@example.com");
            (await GetFieldMessage(page).InputValueAsync()).Should().Be("Format {name} {current_total}");
            (await GetTextMessagePreview(page).InputValueAsync()).Should().Be("Format Example 1\nFormat Example 2\nFormat Example 3\nFormat Example 4\nFormat Example 5\nFormat Example 6\nFormat Example 7\nFormat Example 8\nFormat Example 9\nFormat Example 10");
        });
    }

    // Button
    private static ILocator GetButtonSave(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Save" });

    // Text
    private static ILocator GetTextHeader(IPage page) => page.Locator("h1");
    private static ILocator GetTextMessagePreview(IPage page) => page.Locator(".message-preview__textarea textarea");

    // Field
    private static ILocator GetFieldName(IPage page) => page.GetByLabel("Author Full Name");
    private static ILocator GetFieldEmail(IPage page) => page.GetByLabel("Author Email");
    private static ILocator GetFieldMessage(IPage page) => page.GetByLabel("Message Format");
}
