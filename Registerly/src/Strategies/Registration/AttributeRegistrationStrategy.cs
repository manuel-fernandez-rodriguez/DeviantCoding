using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Registration;

public class AttributeRegistrationStrategy : IRegistrationStrategy
{
    public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            GetRegistrationStrategy(descriptor)
                .RegisterServices(serviceCollection, [descriptor]);
        }

        return serviceCollection;
    }

    private IRegistrationStrategy GetRegistrationStrategy(ServiceDescriptor descriptor)
        => descriptor.ImplementationType?
                .GetCustomAttribute<RegisterlyAttribute>()?
                .RegistrationStrategy ?? new AddRegistrationStrategy();

}