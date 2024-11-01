﻿using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;

public static class WithFactory
{
    public static WithFactory<TService> Create<TService>(Func<IServiceProvider, TService> factory)
        where TService : notnull
        => new(factory);
}

public class WithFactory<TService>(Func<IServiceProvider, TService> factory) : IMappingStrategy
    where TService : notnull
{
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return new ServiceDescriptor(typeof(TService), factory: s => factory(s), lifetimeStrategy.Map(implementationType));
        }
    }
}
