using Microsoft.AspNetCore.Hosting;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public sealed class WebApplicationFixture : IDisposable
{
    public const string BASE_URL = "https://localhost:5000";

    private readonly ActivityPaintWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;


    public WebApplicationFixture()
    {
        _factory = new ActivityPaintWebApplicationFactory();

        _httpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseUrls(BASE_URL);
        }).CreateDefaultClient();
    }

    public void Dispose()
    {
        _httpClient.CancelPendingRequests();
        _httpClient.Dispose();
        _factory.Dispose();
    }
}
