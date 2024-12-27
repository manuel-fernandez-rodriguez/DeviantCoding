using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;

/// <summary>
/// Maps implementation types to a specified service type.
/// </summary>
/// <typeparam name="T">The service type to map to.</typeparam>
/// <example>
/// <code>
/// public class MyService
/// {
///     // Implementation
/// }
///
/// var mappingStrategy = new As&lt;IMyService&gt;();
/// </code>
/// </example>
public class As<T>() : As(typeof(T))
{
}

/// <summary>
/// Maps implementation types to a specified service type.
/// </summary>
/// <param name="serviceType">The service type to map to.</param>
/// <example>
/// <code>
/// public class MyService
/// {
///     // Implementation
/// }
///
/// var mappingStrategy = new As(typeof(IMyService));
/// </code>
/// </example>
public class As(Type serviceType) : IMappingStrategy
{
    /// <summary>
    /// Maps the specified implementation types to a collection of <see cref="ServiceDescriptor"/> instances using the provided lifetime strategy.
    /// </summary>
    /// <param name="implementationTypes">The implementation types to map.</param>
    /// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
    /// <returns>A collection of <see cref="ServiceDescriptor"/> instances.</returns>
    /// <example>
    /// <code>
    /// var mappingStrategy = new As(typeof(IMyService));
    /// var descriptors = mappingStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
    /// </code>
    /// </example>
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return ServiceDescriptorFactory.Create(serviceType, implementationType, lifetimeStrategy);
        }
    }
}
