using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;
public class AsSelf : IMappingStrategy
{
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return new ServiceDescriptor(implementationType, implementationType, lifetimeStrategy.Map(implementationType));
        }
    }
}
