using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Client.Components.Shared.Interops;

namespace ActivityPaint.Client.Components.Shared.Services;

public interface IAppInitializationService
{
    Task InitAsync(CancellationToken cancellationToken = default);
}

internal class AppInitializationService : IAppInitializationService
{
    private readonly IDatabaseStorageInterop _databaseStorageInterop;
    private readonly IDatabaseConfigService _databaseConfigService;

    public AppInitializationService(IDatabaseStorageInterop databaseStorageInterop, IDatabaseConfigService databaseConfigService)
    {
        _databaseStorageInterop = databaseStorageInterop;
        _databaseConfigService = databaseConfigService;
    }

    public async Task InitAsync(CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsBrowser())
        {
            var dbPath = _databaseConfigService.GetDatabasePath();
            await _databaseStorageInterop.SynchronizeFileWithIndexedDb(dbPath, cancellationToken);
        }

        await _databaseConfigService.EnsureCreatedAsync(cancellationToken);
    }
}
