using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class AsSelf : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
        {
            return [new ServiceDescriptor(implementationType, implementationType, lifetimeStrategy.Map(implementationType))];
        }
    }
}
