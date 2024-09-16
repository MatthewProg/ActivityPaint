using System.Collections;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public class AllBrowsersData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { BrowserEnum.Chromium };
        yield return new object[] { BrowserEnum.Firefox };

        if (!OperatingSystem.IsWindows())
        {
            // Playwright Webkit + WASM does not work on Windows
            yield return new object[] { BrowserEnum.Webkit };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
