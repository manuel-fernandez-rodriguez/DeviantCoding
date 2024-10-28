using DeviantCoding.Registerly.SelfRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Registration;

public class AttributeRegistrationStrategy : IRegistrationStrategy
{
    public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            descriptor.ImplementationType?
                .GetCustomAttribute<RegisterlyAttribute>()?
                .RegistrationStrategy
                .RegisterServices(serviceCollection, [descriptor]);
        }

        return serviceCollection;
    }
}