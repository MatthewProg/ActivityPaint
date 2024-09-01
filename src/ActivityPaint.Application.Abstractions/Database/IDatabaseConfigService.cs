namespace ActivityPaint.Application.Abstractions.Database;

public interface IDatabaseConfigService
{
    string GetDatabasePath();
    Task EnsureCreatedAsync(CancellationToken cancellationToken = default);
}
