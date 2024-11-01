using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;

public interface IMappingStrategy
{
    IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy);
}