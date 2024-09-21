using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using WebProgram = ActivityPaint.Client.Web.E2EServer.Program;

namespace ActivityPaint.Client.Web.E2ETests.Setup;

public sealed class ActivityPaintWebApplicationFactory : WebApplicationFactory<WebProgram>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = base.CreateHost(builder);

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        var host = builder.Build();

        host.Start();

        return new CompositeHost(testHost, host);
    }

    public class CompositeHost(IHost testHost, IHost kestrelHost) : IHost
    {
        private readonly IHost _testHost = testHost;
        private readonly IHost _kestrelHost = kestrelHost;

        public IServiceProvider Services => _testHost.Services;

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await _testHost.StartAsync(cancellationToken);
            await _kestrelHost.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await _testHost.StopAsync(cancellationToken);
            await _kestrelHost.StopAsync(cancellationToken);
        }

        public void Dispose()
        {
            _testHost.Dispose();
            _kestrelHost.Dispose();
        }
    }
}
