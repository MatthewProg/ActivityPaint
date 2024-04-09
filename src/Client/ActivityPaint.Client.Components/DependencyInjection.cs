using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace ActivityPaint.Client.Components;

public static class DependencyInjection
{
    public static void AddClientComponents(this IServiceCollection services)
    {
        services.AddMudServices();
    }
}
