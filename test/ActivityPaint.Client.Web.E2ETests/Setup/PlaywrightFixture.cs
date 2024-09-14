using Microsoft.Playwright;
using Xunit.Abstractions;
using PlaywrightProgram = Microsoft.Playwright.Program;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public enum BrowserEnum
{
    Chromium,
    Firefox,
    Webkit
}

public sealed class PlaywrightFixture() : IAsyncLifetime
{
    private static int i = 0;
    private ITestOutputHelper? _testOutputHelper;

    public IPlaywright Playwright { get; private set; } = null!;
    public Lazy<Task<IBrowser>> ChromiumBrowser { get; private set; } = null!;
    public Lazy<Task<IBrowser>> FirefoxBrowser { get; private set; } = null!;
    public Lazy<Task<IBrowser>> WebkitBrowser { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        //InstallPlaywright();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        var defaultSettings = new BrowserTypeLaunchOptions()
        {
            SlowMo = 100
        };

        ChromiumBrowser = new(Playwright.Chromium.LaunchAsync(defaultSettings));
        FirefoxBrowser = new(Playwright.Firefox.LaunchAsync(defaultSettings));
        WebkitBrowser = new(Playwright.Webkit.LaunchAsync(defaultSettings));
    }

    public void SetOutputHelper(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public async Task Run(BrowserEnum browserEnum, string url, Func<IPage, Task> testHandler)
    {
        var browser = await GetBrowser(browserEnum);

        await using var context = await browser.NewContextAsync(new()
        {
            IgnoreHTTPSErrors = true,
        });

        context.Console += LogConsole;

        var page = await context.NewPageAsync();
        ArgumentNullException.ThrowIfNull(page);

        try
        {
            var load = await page.GotoAsync(url, new PageGotoOptions()
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
            ArgumentNullException.ThrowIfNull(load);

            await load.FinishedAsync();
            var a = Interlocked.Increment(ref i);
            await page.ScreenshotAsync(new() { Path = $"./screenshots/sample{a}.png" });

            await testHandler(page);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    private void LogConsole(object? sender, IConsoleMessage e)
    {
        if (e.Type is "trace" or "debug" or "log" or "assert" or "info")
        {
            return;
        }

        _testOutputHelper?.WriteLine($"[{e.Type}] {e.Text}");
    }

    private Task<IBrowser> GetBrowser(BrowserEnum browser) => browser switch
    {
        BrowserEnum.Chromium => ChromiumBrowser.Value,
        BrowserEnum.Firefox => FirefoxBrowser.Value,
        BrowserEnum.Webkit => WebkitBrowser.Value,
        _ => throw new NotImplementedException()
    };

    private static void InstallPlaywright()
    {
        var exitCode = PlaywrightProgram.Main(["install-deps"]);
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode} on install-deps");
        }

        exitCode = PlaywrightProgram.Main(["install"]);
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode} on install");
        }
    }

    public async Task DisposeAsync()
    {
        Playwright?.Dispose();

        if (ChromiumBrowser?.IsValueCreated == true)
        {
            var browser = await ChromiumBrowser.Value;
            await browser.CloseAsync();
            await browser.DisposeAsync();
        }
    }
}
