using ActivityPaint.Client.Web.E2ETests.Extensions;
using ActivityPaint.Client.Web.E2ETests.Setup;
using Microsoft.Playwright;
using System.Net.Mime;
using System.Text;
using Xunit.Abstractions;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class EditorTests : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright;
    private readonly WebApplicationFixture _app;

    public EditorTests(WebApplicationFixture app, PlaywrightFixture playwright, ITestOutputHelper testOutputHelper)
    {
        _playwright = playwright;
        _playwright.SetOutputHelper(testOutputHelper);
        _app = app;
    }

    [Theory]
    [ClassData(typeof(AllBrowsersData))]
    public async Task EditorPage_ShouldPassNewWorkflow(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/");

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert - load editor page
            (await page.Locator("h1").TextContentAsync()).Should().Be("Editor");
            (await page.GetByLabel("Name").InputValueAsync()).Should().BeEmpty();
            (await page.GetByLabel("Picked year").InputValueAsync()).Should().Be(DateTime.Now.Year.ToString());

            // Assert - name required
            await page.GetByRole(AriaRole.Button, new() { Name = "Next stage" }).ClickAsync();
            (await page.GetByText("Preset name is required!").IsVisibleAsync()).Should().BeTrue();
            await page.GetByLabel("Name").FillAsync("Test");

            // Assert - theme mode text change
            (await page.GetByLabel("Light mode is default").IsVisibleAsync()).Should().BeTrue();
            await page.EvalOnSelectorAsync("input[type=checkbox]", "el => el.click()"); // Normal click does not work
            (await page.GetByLabel("Dark mode is default").IsVisibleAsync()).Should().BeTrue();

            // Assert - year pick
            await page.GetByLabel("Open Date Picker").ClickAsync();
            await page.GetByText("2020").ClickAsync();
            (await page.GetByLabel("Picked year").InputValueAsync()).Should().Be("2020");
            await page.GetByRole(AriaRole.Button, new() { Name = "Next stage" }).ClickAsync();
            await Task.Delay(500);

            // Assert - brush size
            (await page.Locator(".brush-size__input input").InputValueAsync()).Should().Be("1");
            await page.Locator(".brush-size__input input").FillAsync("50");
            await page.Locator(".brush-size__input input").BlurAsync();
            (await page.Locator(".brush-size__input input").InputValueAsync()).Should().Be("30");
            await page.Locator(".brush-size__input input").FillAsync("0");
            await page.Locator(".brush-size__input input").BlurAsync();
            (await page.Locator(".brush-size__input input").InputValueAsync()).Should().Be("1");
            await page.Locator("button:has(+ .brush-size__input)").ClickAsync(new() { ClickCount = 2 });
            (await page.Locator(".brush-size__input input").InputValueAsync()).Should().Be("3");

            // Assert - paint
            (await page.EvaluateAsync<string[]>("Array.from(document.querySelectorAll('#paint-canvas td[data-doy]')).map(x => x.dataset.level)")).Should().AllBe("0");
            await page.Locator("div[role=toolbar] .mud-toggle-group:nth-child(3) > div:nth-child(5)").ClickAsync();
            await page.Locator("#cell-3-5 div").ClickAsync();
            await page.Locator("#cell-3-2 div").ClickAsync();
            await page.Locator("div[role=toolbar] .mud-toggle-group:nth-child(3) > div:nth-child(4)").ClickAsync();
            await page.Locator("#cell-7-1 div").ClickAsync();
            await page.Locator("#cell-7-4 div").ClickAsync();
            await page.Locator("div[role=toolbar] .mud-toggle-group:nth-child(3) > div:nth-child(3)").ClickAsync();
            await page.Locator("#cell-11-5 div").ClickAsync();
            await page.Locator("#cell-11-2 div").ClickAsync();
            await page.Locator("div[role=toolbar] .mud-toggle-group:nth-child(3) > div:nth-child(2)").ClickAsync();
            await page.DragAndDropStepsAsync("#cell-15-1 div", "#cell-15-5 div", 1);
            await page.Locator(".brush-size__input + button").ClickAsync(new() { ClickCount = 2 });
            (await page.Locator(".brush-size__input input").InputValueAsync()).Should().Be("1");
            await page.Locator("div[role=toolbar] .mud-toggle-group:first-child > div:nth-child(2)").ClickAsync();
            await page.Locator("#cell-15-3 div").ClickAsync();
            await page.Locator("div[role=toolbar] .mud-toggle-group:first-child > div:nth-child(3)").ClickAsync();
            await page.Locator("#cell-0-3 div").ClickAsync();
            (await page.EvaluateAsync<string>("Array.from(document.querySelectorAll('#paint-canvas td[data-doy]')).map(x => x.dataset.level).reduce((x,y) => x+y)")).Should().Be("111113331111111100000000000000000000000000000000000014441333122211110000000000000000000000000000000000001444133312221111000000000000000000000000000000000000114441333122211010000000000000000000000000000000000001144413331222111100000000000000000000000000000000000011444133312221111000000000000000000000000000000000001144411111222111100000000000000000000000000000000000");
            await page.GetByRole(AriaRole.Button, new() { Name = "Next stage" }).ClickAsync();
            await Task.Delay(500);

            // Assert - generate default
            (await page.GetByRole(AriaRole.Heading, new() { Name = "Please select the method first" }).IsVisibleAsync()).Should().BeTrue();
            await page.GetByText("CLI", new() { Exact = true }).ClickAsync();
            (await page.GetByRole(AriaRole.Heading, new() { Name = "Please select the method first" }).IsVisibleAsync()).Should().BeTrue();

            // Assert - CLI generate git commands
            await page.GetByRole(AriaRole.Radio, new() { Name = "Generate git commands" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Generate commands" }).ClickAsync();
            (await page.Locator(".generate-commands__textarea textarea").InputValueAsync()).Should().Be("ap-cli.exe git --output \"Test.txt\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");
            var commandText = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Save as file" }).ClickAsync();
            });
            commandText.SuggestedFilename.Should().Be("save.txt");
            (await GetFileContentAsync(commandText)).Should().Be("ap-cli.exe git --output \"Test.txt\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");

            // Assert - CLI generate and download repo
            await page.GetByRole(AriaRole.Radio, new() { Name = "Generate and download repository" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Generate commands" }).ClickAsync();
            (await page.Locator(".generate-commands__textarea textarea").InputValueAsync()).Should().Be("ap-cli.exe generate --author-name \"Activity Paint\" --author-email \"email@example.com\" --zip --output \"Test.zip\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");

            // Assert - CLI save preset to file
            await page.GetByRole(AriaRole.Radio, new() { Name = "Save preset to file" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Generate commands" }).ClickAsync();
            (await page.Locator(".generate-commands__textarea textarea").InputValueAsync()).Should().Be("ap-cli.exe save --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w== --dark-mode --output \"Test.json\"");

            // Assert - APP save preset to file
            await page.GetByText("App", new() { Exact = true }).ClickAsync();
            var presetDownload = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Save preset" }).ClickAsync();
            });
            presetDownload.SuggestedFilename.Should().Be("Test.json");
            (await GetFileContentAsync(presetDownload)).Should().Be("{\"Name\":\"Test\",\"StartDate\":\"2020-01-01T00:00:00\",\"IsDarkModeDefault\":true,\"CanvasData\":\"eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==\"}");

            // Assert - APP generate and download repo
            await page.GetByRole(AriaRole.Radio, new() { Name = "Generate and download repository" }).ClickAsync();
            (await page.GetByRole(AriaRole.Heading, new() { Name = "Generation in the browser is not supported yet." }).IsVisibleAsync()).Should().BeTrue();

            // Assert - APP generate git commands
            await page.GetByRole(AriaRole.Radio, new() { Name = "Generate git commands" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Generate commands" }).ClickAsync();
            (await page.Locator(".generate-commands__textarea textarea").InputValueAsync()).Should().StartWith("git commit --allow-empty --no-verify --date=2020-01-01T12:00:00.0000000+00:00 -m \"ActivityPaint - 'Test' - (Commit 1/223)\";");
        });
    }

    [Theory]
    [ClassData(typeof(AllBrowsersData))]
    public async Task EditorPage_ShouldUploadAndParsePreset(BrowserEnum browser)
    {
        // Arrange
        var url = WebApplicationFixture.GetUrl("/");
        var contentBytes = Encoding.UTF8.GetBytes("{\"Name\":\"Test\",\"StartDate\":\"2020-01-01T00:00:00\",\"IsDarkModeDefault\":true,\"CanvasData\":\"eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==\"}");
        var uploadFile = new FilePayload()
        {
            Name = "file.json",
            MimeType = MediaTypeNames.Application.Json,
            Buffer = [..Encoding.UTF8.GetPreamble(), ..contentBytes]
        };

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert - load editor page
            (await page.Locator("h1").TextContentAsync()).Should().Be("Editor");
            (await page.GetByLabel("Name").InputValueAsync()).Should().BeEmpty();
            (await page.GetByLabel("Picked year").InputValueAsync()).Should().Be(DateTime.Now.Year.ToString());

            // Load file and wait to be processed
            await page.Locator("input[type=file]").SetInputFilesAsync(uploadFile);
            await Task.Delay(1000);

            // Assert - file parsed
            (await page.EvaluateAsync<string>("Array.from(document.querySelectorAll('#paint-canvas td[data-doy]')).map(x => x.dataset.level).reduce((x,y) => x+y)")).Should().Be("111113331111111100000000000000000000000000000000000014441333122211110000000000000000000000000000000000001444133312221111000000000000000000000000000000000000114441333122211010000000000000000000000000000000000001144413331222111100000000000000000000000000000000000011444133312221111000000000000000000000000000000000001144411111222111100000000000000000000000000000000000");
            await page.GetByRole(AriaRole.Button, new() { Name = "Reset" }).ClickAsync();
            (await page.EvaluateAsync<string[]>("Array.from(document.querySelectorAll('#paint-canvas td[data-doy]')).map(x => x.dataset.level)")).Should().AllBe("0");
            await page.GetByRole(AriaRole.Button, new() { Name = "Previous stage" }).ClickAsync();
            (await page.GetByLabel("Name").InputValueAsync()).Should().Be("Test");
            (await page.GetByLabel("Dark mode is default").IsVisibleAsync()).Should().BeTrue();
            (await page.GetByLabel("Picked year").InputValueAsync()).Should().Be("2020");
        });
    }

    private static async Task<string> GetFileContentAsync(IDownload download)
    {
        await using var readStream = await download.CreateReadStreamAsync();
        using var memoryStream = new MemoryStream();

        await readStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var bomBytes = Encoding.UTF8.GetPreamble();
        var contentBytes = memoryStream.ToArray();

        bool hasBom = false;
        if (contentBytes.Length >= bomBytes.Length)
        {
            hasBom = true;
            for (int i = 0; i < bomBytes.Length; i++)
            {
                if (bomBytes[i] != contentBytes[i])
                {
                    hasBom = false;
                    break;
                }
            }
        }

        var text = hasBom
            ? Encoding.UTF8.GetString(contentBytes, bomBytes.Length, contentBytes.Length - bomBytes.Length)
            : Encoding.UTF8.GetString(contentBytes);

        return text;
    }
}
