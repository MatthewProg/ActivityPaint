using ActivityPaint.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddCore();
    }
}
