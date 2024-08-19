using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Integration.Repository.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Integration.Repository;

public static class DependencyInjection
{
    public static void AddRepositoryIntegration(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryService, RepositoryService>();
    }
}
