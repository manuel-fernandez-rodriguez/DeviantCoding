using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping;

/// <summary>
/// Provides a factory method for creating <see cref="WithFactory{TService}"/> instances.
/// </summary>
public static class WithFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="WithFactory{TService}"/> with the specified factory function.
    /// </summary>
    /// <typeparam name="TService">The type of service to create.</typeparam>
    /// <param name="factory">The factory function to use for creating the service.</param>
    /// <returns>A new instance of <see cref="WithFactory{TService}"/>.</returns>
    /// <example>
    /// <code>
    /// var factoryStrategy = WithFactory.Create(s => new MyService());
    /// </code>
    /// </example>
    public static WithFactory<TService> Create<TService>(Func<IServiceProvider, TService> factory)
        where TService : notnull
        => new(factory);
}

/// <summary>
/// Maps implementation types to service descriptors using a factory function.
/// </summary>
/// <typeparam name="TService">The type of service to create.</typeparam>
/// <param name="factory">The factory function to use for creating the service.</param>
/// <example>
/// <code>
/// public class MyService
/// {
///     // Implementation
/// }
///
/// var factoryStrategy = new WithFactory&lt;MyService&gt;(s => new MyService());
/// var descriptors = factoryStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
/// </code>
/// </example>
public class WithFactory<TService>(Func<IServiceProvider, TService> factory) : IMappingStrategy
    where TService : notnull
{
    /// <summary>
    /// Maps the specified implementation types to a collection of <see cref="ServiceDescriptor"/> instances using the provided lifetime strategy.
    /// </summary>
    /// <param name="implementationTypes">The implementation types to map.</param>
    /// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
    /// <returns>A collection of <see cref="ServiceDescriptor"/> instances.</returns>
    /// <example>
    /// <code>
    /// var factoryStrategy = new WithFactory&lt;MyService&gt;(s => new MyService());
    /// var descriptors = factoryStrategy.Map(new[] { typeof(MyService) }, new SingletonLifetimeStrategy());
    /// </code>
    /// </example>
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return new ServiceDescriptor(typeof(TService), factory: s => factory(s), lifetimeStrategy.Map(implementationType));
        }
    }
}
