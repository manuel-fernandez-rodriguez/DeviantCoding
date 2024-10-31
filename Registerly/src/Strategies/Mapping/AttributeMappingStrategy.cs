using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping;

internal class AttributeMappingStrategy : IMappingStrategy
{
    private static readonly AsImplementedInterfaces _asImplementedInterfaces = new();
    private static readonly AsSelf _asSelf = new();

    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        
        foreach(var implementationType in implementationTypes)
        {
            var mappingStrategy = implementationType
               .GetCustomAttribute<RegisterlyAttribute>()?
               .MappingStrategy;

            if (mappingStrategy != null)
            {
                foreach (var serviceDescriptor in mappingStrategy.Map([implementationType], lifetimeStrategy))
                {
                    yield return serviceDescriptor;
                }
            }

            if (implementationType.GetInterfaces().Length != 0)
            {
                foreach (var serviceDescriptor in _asImplementedInterfaces.Map([implementationType], lifetimeStrategy))
                {
                    yield return serviceDescriptor;
                }
            }

            foreach (var serviceDescriptor in _asSelf.Map([implementationType], lifetimeStrategy))
            {
                yield return serviceDescriptor;
            }
        }

        
    }
}

