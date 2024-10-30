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
            var registrationStrategy = descriptor.ImplementationType?
                .GetCustomAttribute<RegisterlyAttribute>()?
                .RegistrationStrategy;
            
            if (registrationStrategy != null)
            {
                registrationStrategy.RegisterServices(serviceCollection, [descriptor]);
                continue;
            }
            
            new AddRegistrationStrategy().RegisterServices(serviceCollection, [descriptor]);
        }

        return serviceCollection;
    }
}