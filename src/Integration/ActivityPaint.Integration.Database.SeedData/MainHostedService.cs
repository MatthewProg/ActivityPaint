using Microsoft.Extensions.Hosting;

namespace ActivityPaint.Integration.Database.SeedData;

internal class MainHostedService : IHostedService
{
    private readonly IDatabaseSeedService _databaseSeedService;

    public MainHostedService(IDatabaseSeedService databaseSeedService)
    {
        _databaseSeedService = databaseSeedService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _databaseSeedService.SeedAsync(cancellationToken);
        Environment.Exit(0);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
