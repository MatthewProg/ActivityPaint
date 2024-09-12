using Microsoft.AspNetCore.Hosting;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public sealed class WebApplicationFixture : IDisposable
{
    public const string BASE_URL = "https://localhost:5000";

    private static readonly Uri _baseUri = new(BASE_URL);
    private readonly ActivityPaintWebApplicationFactory _factory;

    public HttpClient HttpClient { get; private init; }

    public WebApplicationFixture()
    {
        _factory = new ActivityPaintWebApplicationFactory();

        HttpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseUrls(BASE_URL);
        }).CreateDefaultClient();
    }

    public static string GetUrl(string relative)
        => new Uri(_baseUri, relative).ToString();

    public void Dispose()
    {
        HttpClient.CancelPendingRequests();
        HttpClient.Dispose();
        _factory.Dispose();
    }
}
