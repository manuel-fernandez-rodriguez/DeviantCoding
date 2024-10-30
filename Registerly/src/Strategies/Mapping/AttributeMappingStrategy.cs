using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping;

internal class AttributeMappingStrategy : IMappingStrategy
{
    public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
    {
        var mappingStrategy = implementationType
            .GetCustomAttribute<RegisterlyAttribute>()?
            .MappingStrategy;

        if (mappingStrategy != null)
        {
            return mappingStrategy.Map(implementationType, lifetimeStrategy);
        }

        if (implementationType.GetInterfaces().Length != 0)
        {
            return new AsImplementedInterfaces().Map(implementationType, lifetimeStrategy);
        }

        return new AsSelf().Map(implementationType, lifetimeStrategy);
    }
}

