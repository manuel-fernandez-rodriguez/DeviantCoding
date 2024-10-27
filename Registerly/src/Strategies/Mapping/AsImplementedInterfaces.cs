using DeviantCoding.Registerly.Strategies.Lifetime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class AsImplementedInterfaces : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
        {
            var services = implementationType.GetInterfaces();
            return services.Select(service => new ServiceDescriptor(service, implementationType, lifetimeStrategy.Lifetime));
        }
    }
}
