using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityPaint.Application.DTOs;

public static class DependencyInjection
{
    public static void AddDTOs(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), ServiceLifetime.Transient);
    }
}
