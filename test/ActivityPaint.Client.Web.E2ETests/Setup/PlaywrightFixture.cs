using Microsoft.Playwright;
using PlaywrightProgram = Microsoft.Playwright.Program;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public enum BrowserEnum
{
    Chromium,
    Firefox,
    Webkit
}

public sealed class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;
    public Lazy<Task<IBrowser>> ChromiumBrowser { get; private set; } = null!;
    public Lazy<Task<IBrowser>> FirefoxBrowser { get; private set; } = null!;
    public Lazy<Task<IBrowser>> WebkitBrowser { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        InstallPlaywright();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        ChromiumBrowser = new(Playwright.Chromium.LaunchAsync());
        FirefoxBrowser = new(Playwright.Firefox.LaunchAsync());
        WebkitBrowser = new(Playwright.Webkit.LaunchAsync());
    }

    public async Task Run(BrowserEnum browserEnum, string url, Func<IPage, Task> testHandler)
    {
        var browser = await GetBrowser(browserEnum);

        await using var context = await browser.NewContextAsync(new()
        {
            IgnoreHTTPSErrors = true,
        });

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

            await testHandler(page);
        }
        finally
        {
            await page.CloseAsync();
        }
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
