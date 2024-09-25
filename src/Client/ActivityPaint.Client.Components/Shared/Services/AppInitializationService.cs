using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Client.Components.Shared.Interops;

namespace ActivityPaint.Client.Components.Shared.Services;

public interface IAppInitializationService
{
    Task InitAsync(CancellationToken cancellationToken = default);
}

internal class AppInitializationService(IDatabaseStorageInterop databaseStorageInterop, IDatabaseConfigService databaseConfigService) : IAppInitializationService
{
    private readonly IDatabaseStorageInterop _databaseStorageInterop = databaseStorageInterop;
    private readonly IDatabaseConfigService _databaseConfigService = databaseConfigService;

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
