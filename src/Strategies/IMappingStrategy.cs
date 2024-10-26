using DeviantCoding.Registerly.Strategies.Lifetime;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;

public interface IMappingStrategy
{
    IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy);
}