using ActivityPaint.Client.Web.E2ETests.Setup;
using Microsoft.Playwright;
using System.Net.Mime;
using System.Text;

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
        var shareUrl = "https://localhost:5000/editor?name=Test&startDate=2024-01-01&data=eAFsUEEOACAIstb%2F3xwKVs44yFRgTrOOSQyi7hdgXoLqTt0iJ1Gc3dchw1FCjHirBJEPnaK2QkvzPSmZ2Kw3VNk6JbufQbNQno9UoT6of2K3AQAA%2F%2F8%3D&darkMode=True";
        var contentBytes = Encoding.UTF8.GetBytes("{\"Name\":\"Test\",\"StartDate\":\"2020-01-01T00:00:00\",\"IsDarkModeDefault\":true,\"CanvasData\":\"eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==\"}");
        var uploadFile = new FilePayload()
        {
            Name = "file.json",
            MimeType = MediaTypeNames.Application.Json,
            Buffer = [.. Encoding.UTF8.GetPreamble(), .. contentBytes]
        };

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert - page load
            (await GetTextPageHeader(page).TextContentAsync()).Should().Be("Gallery");
            await Task.Delay(1000);
            (await GetTextEmptyList(page).IsVisibleAsync()).Should().BeTrue();

            // Assert - preset upload
            await GetFieldFile(page).SetInputFilesAsync(uploadFile);
            await Task.Delay(1000);
            (await GetTextHeader(page, "Test").IsVisibleAsync()).Should().BeTrue();
            await GetGalleryItem(page).HoverAsync();
            await GetButtonDelete(page).ClickAsync();
            await GetButtonDeleteConfirm(page).ClickAsync();

            // Assert - add by url
            await GetFieldInputShareUrl(page).ClickAsync();
            await GetFieldInputShareUrl(page).FillAsync(shareUrl);
            await GetButtonAdd(page).ClickAsync();
            await Task.Delay(1000);

            // Assert - item
            (await GetTextHeader(page, "Test").IsVisibleAsync()).Should().BeTrue();
            (await page.GetByRole(AriaRole.Img).IsVisibleAsync()).Should().BeTrue();
            (await page.GetByText("Start date: 2024-01-01").IsVisibleAsync()).Should().BeTrue();
            (await page.GetByText("Default theme: Dark").IsVisibleAsync()).Should().BeTrue();

            // Assert - share
            await GetButtonShare(page).ClickAsync();
            (await GetFieldShareUrl(page).InputValueAsync()).Should().Be(shareUrl);
            await GetButtonShareCopy(page).ClickAsync();

            // Assert - save
            await GetButtonSave(page).ClickAsync();
            (await GetTextHeader(page, "Save preset").IsVisibleAsync()).Should().BeTrue();
            await GetButtonCloseDialog(page).ClickAsync();

            // Assert - edit
            await GetButtonEdit(page).ClickAsync();
            new Uri(page.Url).LocalPath.Should().StartWith("/editor");
        });
    }

    // Elements
    private static ILocator GetGalleryItem(IPage page) => page.Locator(".item-container").First;

    // Text
    private static ILocator GetTextPageHeader(IPage page) => page.Locator("h1");
    private static ILocator GetTextHeader(IPage page, string text) => page.GetByRole(AriaRole.Heading, new() { Name = text });
    private static ILocator GetTextEmptyList(IPage page) => page.GetByRole(AriaRole.Heading, new() { Name = "No saved presets yet" });

    // Fields
    private static ILocator GetFieldFile(IPage page) => page.Locator("input[type=file]");
    private static ILocator GetFieldInputShareUrl(IPage page) => page.GetByPlaceholder("Share URL");
    private static ILocator GetFieldShareUrl(IPage page) => page.Locator(".mud-popover-provider .mud-input > input");

    // Buttons
    private static ILocator GetButtonDelete(IPage page) => page.Locator(".mud-card-header-actions > .mud-button-root").First;
    private static ILocator GetButtonDeleteConfirm(IPage page) => page.Locator(".mud-card-header-actions > button").First;
    private static ILocator GetButtonAdd(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Add" });
    private static ILocator GetButtonShare(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Share" });
    private static ILocator GetButtonEdit(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
    private static ILocator GetButtonSave(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Save" });
    private static ILocator GetButtonShareCopy(IPage page) => page.Locator(".mud-popover-provider .mud-tooltip-root > button");
    private static ILocator GetButtonCloseDialog(IPage page) => page.GetByLabel("Close dialog");
}
