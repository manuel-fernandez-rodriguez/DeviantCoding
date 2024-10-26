using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class AsSelf : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ServiceLifetime serviceLifetime)
        {
            return [new ServiceDescriptor(implementationType, implementationType, serviceLifetime)];
        }

        public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors, ServiceLifetime serviceLifetime)
        {
            throw new NotImplementedException();
        }
    }
}
