using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Config.DependencyInjection;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    public object? Resolve(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return _provider.GetService(type);
    }
}
