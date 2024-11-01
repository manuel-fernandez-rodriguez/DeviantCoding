﻿using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;
public class AsImplementedInterfaces : IMappingStrategy
{
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            var services = implementationType.GetInterfaces();
            foreach (var service in services)
            {
                yield return new ServiceDescriptor(service, implementationType, lifetimeStrategy.Map(implementationType));
            }
        }
    }
}
