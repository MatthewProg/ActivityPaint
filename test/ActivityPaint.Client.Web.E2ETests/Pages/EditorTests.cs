using ActivityPaint.Client.Web.E2ETests.Extensions;
using ActivityPaint.Client.Web.E2ETests.Setup;
using ActivityPaint.Core.Enums;
using Microsoft.Playwright;
using System.Net.Mime;
using System.Text;

namespace ActivityPaint.Client.Web.E2ETests.Pages;

public class EditorTests(PlaywrightFixture playwright) : IAssemblyFixture<WebApplicationFixture>, IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _playwright = playwright;

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
            (await GetTextHeader(page).TextContentAsync()).Should().Be("Editor");
            (await GetFieldName(page).InputValueAsync()).Should().BeEmpty();
            (await GetFieldYear(page).InputValueAsync()).Should().Be(DateTime.Now.Year.ToString());

            // Assert - name required
            await GetButtonNextStage(page).ClickAsync();
            (await page.GetByText("Preset name is required!").IsVisibleAsync()).Should().BeTrue();
            await GetFieldName(page).FillAsync("Test");

            // Assert - theme mode text change
            (await GetCheckboxDarkMode(page, false).IsVisibleAsync()).Should().BeTrue();
            await page.EvalOnSelectorAsync("input[type=checkbox]", "el => el.click()"); // Normal click does not work
            (await GetCheckboxDarkMode(page, true).IsVisibleAsync()).Should().BeTrue();

            // Assert - year pick
            await page.GetByLabel("Open Date Picker").ClickAsync();
            await page.GetByText("2020").ClickAsync();
            (await GetFieldYear(page).InputValueAsync()).Should().Be("2020");
            await GetButtonNextStage(page).ClickAsync();
            await Task.Delay(500);

            // Assert - brush size
            (await GetFieldBrushSize(page).InputValueAsync()).Should().Be("1");
            await GetFieldBrushSize(page).FillAsync("50");
            await GetFieldBrushSize(page).BlurAsync();
            (await GetFieldBrushSize(page).InputValueAsync()).Should().Be("30");
            await GetFieldBrushSize(page).FillAsync("0");
            await GetFieldBrushSize(page).BlurAsync();
            (await GetFieldBrushSize(page).InputValueAsync()).Should().Be("1");
            await GetButtonBrushSizePlus(page).ClickAsync(new() { ClickCount = 2 });
            (await GetFieldBrushSize(page).InputValueAsync()).Should().Be("3");

            // Assert - paint
            (await GetCanvasContent(page)).Should().Be("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            await GetButtonIntensity(page, IntensityEnum.Level4).ClickAsync();
            await GetCanvasCell(page, 3, 5).ClickAsync();
            await GetCanvasCell(page, 3, 2).ClickAsync();
            await GetButtonIntensity(page, IntensityEnum.Level3).ClickAsync();
            await GetCanvasCell(page, 7, 1).ClickAsync();
            await GetCanvasCell(page, 7, 4).ClickAsync();
            await GetButtonIntensity(page, IntensityEnum.Level2).ClickAsync();
            await GetCanvasCell(page, 11, 5).ClickAsync();
            await GetCanvasCell(page, 11, 2).ClickAsync();
            await GetButtonIntensity(page, IntensityEnum.Level1).ClickAsync();
            await page.DragAndDropStepsAsync(GetCanvasCell(page, 15, 1), GetCanvasCell(page, 15, 5), 1);
            await GetButtonBrushSizeMinus(page).ClickAsync(new() { ClickCount = 2 });
            (await GetFieldBrushSize(page).InputValueAsync()).Should().Be("1");
            await GetButtonToolEraser(page).ClickAsync();
            await GetCanvasCell(page, 15, 3).ClickAsync();
            await GetButtonToolFill(page).ClickAsync();
            await GetCanvasCell(page, 0, 3).ClickAsync();
            (await GetCanvasContent(page)).Should().Be("111113331111111100000000000000000000000000000000000014441333122211110000000000000000000000000000000000001444133312221111000000000000000000000000000000000000114441333122211010000000000000000000000000000000000001144413331222111100000000000000000000000000000000000011444133312221111000000000000000000000000000000000001144411111222111100000000000000000000000000000000000");
            await GetButtonNextStage(page).ClickAsync();
            await Task.Delay(500);

            // Assert - generate default
            (await GetTextMethodNotSelected(page).IsVisibleAsync()).Should().BeTrue();
            await GetCliTab(page).ClickAsync();
            (await GetTextMethodNotSelected(page).IsVisibleAsync()).Should().BeTrue();

            // Assert - CLI generate git commands
            await GetButtonGitCommands(page).ClickAsync();
            await GetButtonGenerateCommands(page).ClickAsync();
            (await GetTextCliCommands(page).InputValueAsync()).Should().Be("ap-cli.exe git --output \"Test.txt\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");
            var commandText = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Save as file" }).ClickAsync();
            });
            commandText.SuggestedFilename.Should().Be("save.txt");
            (await GetFileContentAsync(commandText)).Should().Be("ap-cli.exe git --output \"Test.txt\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");

            // Assert - CLI generate and download repo
            await GetButtonGenerateRepo(page).ClickAsync();
            await GetButtonGenerateCommands(page).ClickAsync();
            (await GetTextCliCommands(page).InputValueAsync()).Should().Be("ap-cli.exe generate --author-name \"Activity Paint\" --author-email \"email@example.com\" --zip --output \"Test.zip\" new --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==");

            // Assert - CLI save preset to file
            await GetButtonSavePreset(page).ClickAsync();
            await GetButtonGenerateCommands(page).ClickAsync();
            (await GetTextCliCommands(page).InputValueAsync()).Should().Be("ap-cli.exe save --name \"Test\" --start-date 2020-01-01 --data eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w== --dark-mode --output \"Test.json\"");

            // Assert - APP save preset to file
            await GetAppTab(page).ClickAsync();
            var presetDownload = await page.RunAndWaitForDownloadAsync(async () =>
            {
                await page.GetByRole(AriaRole.Button, new() { Name = "Save preset" }).ClickAsync();
            });
            presetDownload.SuggestedFilename.Should().Be("Test.json");
            (await GetFileContentAsync(presetDownload)).Should().Be("{\"Name\":\"Test\",\"StartDate\":\"2020-01-01T00:00:00\",\"IsDarkModeDefault\":true,\"CanvasData\":\"eAFiZEQAFjBgRKUg0sxgwIhKwXQygQEjKgWThNEMMAYjI8OIBQAAAAD//w==\"}");

            // Assert - APP generate and download repo
            await GetButtonGenerateRepo(page).ClickAsync();
            (await page.GetByRole(AriaRole.Heading, new() { Name = "Generation in the browser is not supported yet." }).IsVisibleAsync()).Should().BeTrue();

            // Assert - APP generate git commands
            await GetButtonGitCommands(page).ClickAsync();
            await GetButtonGenerateCommands(page).ClickAsync();
            (await GetTextAppCommands(page).InputValueAsync()).Should().StartWith("git commit --allow-empty --no-verify --date=2020-01-01T12:00:00.0000000+00:00 -m \"ActivityPaint - 'Test' - (Commit 1/223)\";");
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
            Buffer = [.. Encoding.UTF8.GetPreamble(), .. contentBytes]
        };

        // Act
        await _playwright.Run(browser, url, async page =>
        {
            // Assert - load editor page
            (await GetTextHeader(page).TextContentAsync()).Should().Be("Editor");
            (await GetFieldName(page).InputValueAsync()).Should().BeEmpty();
            (await GetFieldYear(page).InputValueAsync()).Should().Be(DateTime.Now.Year.ToString());

            // Load file and wait to be processed
            await GetFieldFile(page).SetInputFilesAsync(uploadFile);
            await Task.Delay(1000);

            // Assert - file parsed
            (await GetCanvasContent(page)).Should().Be("111113331111111100000000000000000000000000000000000014441333122211110000000000000000000000000000000000001444133312221111000000000000000000000000000000000000114441333122211010000000000000000000000000000000000001144413331222111100000000000000000000000000000000000011444133312221111000000000000000000000000000000000001144411111222111100000000000000000000000000000000000");
            await GetButtonReset(page).ClickAsync();
            (await GetCanvasContent(page)).Should().Be("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            await GetButtonPreviousStage(page).ClickAsync();
            (await GetFieldName(page).InputValueAsync()).Should().Be("Test");
            (await GetCheckboxDarkMode(page, true).IsVisibleAsync()).Should().BeTrue();
            (await GetFieldYear(page).InputValueAsync()).Should().Be("2020");
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

    // Checkbox
    private static ILocator GetCheckboxDarkMode(IPage page, bool @checked) => @checked
        ? page.GetByLabel("Dark mode is default")
        : page.GetByLabel("Light mode is default");

    // Fields
    private static ILocator GetFieldName(IPage page) => page.GetByLabel("Name");
    private static ILocator GetFieldYear(IPage page) => page.GetByLabel("Picked year");
    private static ILocator GetFieldBrushSize(IPage page) => page.Locator(".brush-size__input input");
    private static ILocator GetFieldFile(IPage page) => page.Locator("input[type=file]");

    // Buttons
    private static ILocator GetButtonPreviousStage(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Previous stage" });
    private static ILocator GetButtonNextStage(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Next stage" });
    private static ILocator GetButtonReset(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Reset" });
    private static ILocator GetButtonBrushSizePlus(IPage page) => page.Locator("button:has(+ .brush-size__input)");
    private static ILocator GetButtonBrushSizeMinus(IPage page) => page.Locator(".brush-size__input + button");
    private static ILocator GetButtonIntensity(IPage page, IntensityEnum intensity) => page.Locator($"div[role=toolbar] .mud-toggle-group:nth-child(3) > button:nth-child({(int)intensity + 1})");
    private static ILocator GetButtonToolBrush(IPage page) => GetButtonTool(page, 0);
    private static ILocator GetButtonToolEraser(IPage page) => GetButtonTool(page, 1);
    private static ILocator GetButtonToolFill(IPage page) => GetButtonTool(page, 2);
    private static ILocator GetButtonTool(IPage page, int index) => page.Locator($"div[role=toolbar] .mud-toggle-group:first-child > button:nth-child({index + 1})");
    private static ILocator GetButtonSavePreset(IPage page) => page.GetByRole(AriaRole.Radio, new() { Name = "Save preset to file" });
    private static ILocator GetButtonGenerateRepo(IPage page) => page.GetByRole(AriaRole.Radio, new() { Name = "Generate and download repository" });
    private static ILocator GetButtonGitCommands(IPage page) => page.GetByRole(AriaRole.Radio, new() { Name = "Generate git commands" });
    private static ILocator GetButtonGenerateCommands(IPage page) => page.GetByRole(AriaRole.Button, new() { Name = "Generate commands" });

    // Canvas
    private static ILocator GetCanvasCell(IPage page, int x, int y) => page.Locator($"#cell-{x}-{y} div");
    private static Task<string> GetCanvasContent(IPage page) => page.EvaluateAsync<string>("Array.from(document.querySelectorAll('#paint-canvas td[data-doy]')).map(x => x.dataset.level).reduce((x,y) => x+y)");

    // Tabs
    private static ILocator GetCliTab(IPage page) => page.GetByText("CLI", new() { Exact = true });
    private static ILocator GetAppTab(IPage page) => page.GetByText("App", new() { Exact = true });

    // Text
    private static ILocator GetTextHeader(IPage page) => page.Locator("h1");
    private static ILocator GetTextMethodNotSelected(IPage page) => page.GetByRole(AriaRole.Heading, new() { Name = "Please select the method first" });
    private static ILocator GetTextAppCommands(IPage page) => page.Locator(".mud-tabs-panels > div:nth-child(1) .generate-commands__textarea textarea");
    private static ILocator GetTextCliCommands(IPage page) => page.Locator(".mud-tabs-panels > div:nth-child(2) .generate-commands__textarea textarea");
}