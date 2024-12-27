using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviantCoding.Registerly.Strategies.Mapping;
internal static class ServiceDescriptorFactory
{
    public static ServiceDescriptor Create(Type serviceType, Type implementationType, ILifetimeStrategy lifetimeStrategy)
    {
        return new ServiceDescriptor(
            serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType,
            implementationType,
            lifetimeStrategy.Map(implementationType));
    }
}
