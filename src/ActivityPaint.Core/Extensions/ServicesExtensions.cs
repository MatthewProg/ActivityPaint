using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Core.Extensions;

public static class ServicesExtensions
{
    public static void ThrowIfNotRegistered<T>(this IServiceCollection services)
    {
        if (!services.Any(x => x.ServiceType == typeof(T)))
        {
            throw new ApplicationException($"Instance of {typeof(T)} must be registered for the application to work.");
        }
    }
}
