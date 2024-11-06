using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Registration;

public class Add : IRegistrationStrategy
{
    public IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            serviceCollection.Add(descriptor);
        }

        return serviceCollection;
    }
}