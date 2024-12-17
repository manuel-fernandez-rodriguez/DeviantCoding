using System.Reflection;
using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Lifetime;

/// <summary>
/// Represents a scoped lifetime strategy.
/// </summary>
/// <example>
/// <code>
/// public class MyScopedService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
public class Scoped() : LifetimeStrategy(ServiceLifetime.Scoped)
{
}

/// <summary>
/// Represents a singleton lifetime strategy.
/// </summary>
/// <example>
/// <code>
/// public class MySingletonService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
public class Singleton() : LifetimeStrategy(ServiceLifetime.Singleton)
{
}

/// <summary>
/// Represents a transient lifetime strategy.
/// </summary>
/// <example>
/// <code>
/// public class MyTransientService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
public class Transient() : LifetimeStrategy(ServiceLifetime.Transient)
{
}

/// <summary>
/// Represents a lifetime strategy based on attributes.
/// </summary>
/// <example>
/// <code>
/// [Singleton]
/// public class MyService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
public class AttributeLifetimeStrategy : ILifetimeStrategy
{
    /// <summary>
    /// Maps the specified implementation type to a <see cref="ServiceLifetime"/> based on attributes.
    /// </summary>
    /// <param name="implementationType">The implementation type to map.</param>
    /// <returns>The <see cref="ServiceLifetime"/> for the specified implementation type.</returns>
    public ServiceLifetime Map(Type implementationType)
    {
        return implementationType
            .GetCustomAttribute<RegisterlyAttribute>()?.
            LifetimeStrategy?.Map(implementationType)
            ?? ServiceLifetime.Scoped;
    }
}

/// <summary>
/// Represents a base class for lifetime strategies.
/// </summary>
/// <param name="lifetime">The <see cref="ServiceLifetime"/> to use.</param>
/// <example>
/// <code>
/// public class CustomLifetimeStrategy : LifetimeStrategy
/// {
///     public CustomLifetimeStrategy() : base(ServiceLifetime.Singleton)
///     {
///     }
/// }
/// </code>
/// </example>
public abstract class LifetimeStrategy(ServiceLifetime lifetime) : ILifetimeStrategy
{
    /// <summary>
    /// Maps the specified implementation type to the configured <see cref="ServiceLifetime"/>.
    /// </summary>
    /// <param name="implementationType">The implementation type to map.</param>
    /// <returns>The <see cref="ServiceLifetime"/> for the specified implementation type.</returns>
    public ServiceLifetime Map(Type implementationType)
    {
        return lifetime;
    }
}
