using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Core;

public static class DependencyInjection
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);
    }
}
