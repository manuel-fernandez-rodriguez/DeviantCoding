using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;
/// <summary>
/// Maps implementation types to their implemented interfaces.
/// </summary>
/// <example>
/// <code>
/// public interface IMyService
/// {
///     // Interface definition
/// }
///
/// public class MyService : IMyService
/// {
///     // Implementation
/// }
///
/// var mappingStrategy = new AsImplementedInterfaces();
/// var descriptors = mappingStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
/// </code>
/// </example>
public class AsImplementedInterfaces : IMappingStrategy
{
    /// <summary>
    /// Maps the specified implementation types to a collection of <see cref="ServiceDescriptor"/> instances using the provided lifetime strategy.
    /// </summary>
    /// <param name="implementationTypes">The implementation types to map.</param>
    /// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
    /// <returns>A collection of <see cref="ServiceDescriptor"/> instances.</returns>
    /// <example>
    /// <code>
    /// var mappingStrategy = new AsImplementedInterfaces();
    /// var descriptors = mappingStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
    /// </code>
    /// </example>
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
