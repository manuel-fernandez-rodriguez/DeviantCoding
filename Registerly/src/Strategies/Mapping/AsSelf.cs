using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;
/// <summary>
/// Maps implementation types to themselves.
/// </summary>
/// <example>
/// <code>
/// public class MyService
/// {
///     // Implementation
/// }
///
/// var mappingStrategy = new AsSelf();
/// var descriptors = mappingStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
/// </code>
/// </example>
public class AsSelf : IMappingStrategy
{
    /// <summary>
    /// Maps the specified implementation types to a collection of <see cref="ServiceDescriptor"/> instances using the provided lifetime strategy.
    /// </summary>
    /// <param name="implementationTypes">The implementation types to map.</param>
    /// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
    /// <returns>A collection of <see cref="ServiceDescriptor"/> instances.</returns>
    /// <example>
    /// <code>
    /// var mappingStrategy = new AsSelf();
    /// var descriptors = mappingStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
    /// </code>
    /// </example>
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return new ServiceDescriptor(implementationType, implementationType, lifetimeStrategy.Map(implementationType));
        }
    }
}
