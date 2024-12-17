using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;
/// <summary>
/// Defines a strategy for determining the lifetime of a service.
/// </summary>
public interface ILifetimeStrategy
{
    /// <summary>
    /// Maps the specified implementation type to a <see cref="ServiceLifetime"/>.
    /// </summary>
    /// <param name="implementationType">The implementation type to map.</param>
    /// <returns>The <see cref="ServiceLifetime"/> for the specified implementation type.</returns>
    /// <example>
    /// <code>
    /// public class SingletonLifetimeStrategy : ILifetimeStrategy
    /// {
    ///     public ServiceLifetime Map(Type implementationType)
    ///     {
    ///         return ServiceLifetime.Singleton;
    ///     }
    /// }
    /// </code>
    /// </example>
    ServiceLifetime Map(Type implementationType);
}
