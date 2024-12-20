using System.Reflection;
using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Registration;

/// <summary>
/// A strategy for registering services based on attributes.
/// </summary>
/// <example>
/// <code>
/// [Registerly(typeof(Singleton), typeof(MyMappingStrategy), typeof(MyRegistrationStrategy))]
/// public class MyService
/// {
///     // Implementation
/// }
///
/// var registrationStrategy = new AttributeRegistrationStrategy();
/// var services = new ServiceCollection();
/// var descriptors = new List&lt;ServiceDescriptor&gt;
/// {
///     new ServiceDescriptor(typeof(IMyService), typeof(MyService), ServiceLifetime.Singleton)
/// };
/// registrationStrategy.RegisterServices(services, descriptors);
/// </code>
/// </example>
public class AttributeRegistrationStrategy : IRegistrationStrategy
{
    /// <summary>
    /// Registers the specified service descriptors in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services in.</param>
    /// <param name="descriptors">The service descriptors to register.</param>
    /// <returns>The service collection with the registered services.</returns>
    /// <example>
    /// <code>
    /// var registrationStrategy = new AttributeRegistrationStrategy();
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
            GetRegistrationStrategy(descriptor)
                .RegisterServices(serviceCollection, [descriptor]);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Gets the registration strategy for the specified service descriptor.
    /// </summary>
    /// <param name="descriptor">The service descriptor to get the registration strategy for.</param>
    /// <returns>The registration strategy for the specified service descriptor.</returns>
    private IRegistrationStrategy GetRegistrationStrategy(ServiceDescriptor descriptor)
        => descriptor.ImplementationType?
                .GetCustomAttribute<RegisterlyAttribute>()?
                .RegistrationStrategy ?? new Add();
}
