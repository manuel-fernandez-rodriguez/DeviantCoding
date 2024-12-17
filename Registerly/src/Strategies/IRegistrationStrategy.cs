using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;

/// <summary>
/// Defines a strategy for registering services in the dependency injection container.
/// </summary>
public interface IRegistrationStrategy
{
    /// <summary>
    /// Registers the specified service descriptors in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services in.</param>
    /// <param name="descriptors">The service descriptors to register.</param>
    /// <returns>The service collection with the registered services.</returns>
    /// <example>
    /// <code>
    /// public class MyRegistrationStrategy : IRegistrationStrategy
    /// {
    ///     public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable&lt;ServiceDescriptor&gt; descriptors)
    ///     {
    ///         foreach (var descriptor in descriptors)
    ///         {
    ///             serviceCollection.Add(descriptor);
    ///         }
    ///         return serviceCollection;
    ///     }
    /// }
    /// </code>
    /// </example>
    IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors);
}
