﻿using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace ActivityPaint.Client.Console.Config.DependencyInjection;

internal sealed class TypeRegistrar(IServiceCollection builder) : ITypeRegistrar
{
    private readonly IServiceCollection _builder = builder;

    public ITypeResolver Build()
    {
        return new TypeResolver(_builder.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        _builder.AddSingleton(service, _ => func());
    }
}
