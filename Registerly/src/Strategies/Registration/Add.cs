using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Registration;

/// <summary>
/// A strategy for adding service descriptors to the service collection.
/// </summary>
/// <example>
/// <code>
/// var registrationStrategy = new Add();
/// var services = new ServiceCollection();
/// var descriptors = new List&lt;ServiceDescriptor&gt;
/// {
///     new ServiceDescriptor(typeof(IMyService), typeof(MyService), ServiceLifetime.Singleton)
/// };
/// registrationStrategy.RegisterServices(services, descriptors);
/// </code>
/// </example>
public class Add : IRegistrationStrategy
{
    /// <summary>
    /// Registers the specified service descriptors in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services in.</param>
    /// <param name="descriptors">The service descriptors to register.</param>
    /// <returns>The service collection with the registered services.</returns>
    /// <example>
    /// <code>
    /// var registrationStrategy = new Add();
    /// var services = new ServiceCollection();
    /// var descriptors = new List&lt;ServiceDescriptor&gt;
    /// {
    ///     new ServiceDescriptor(typeof(IMyService), typeof(MyService), ServiceLifetime.Singleton)
    /// };
    /// registrationStrategy.RegisterServices(services, descriptors);
    /// </code>
    /// </example>
    public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            serviceCollection.Add(descriptor);
        }

        return serviceCollection;
    }
}
