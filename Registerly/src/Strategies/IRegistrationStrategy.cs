using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;

public interface IRegistrationStrategy
{
    IServiceCollection RegisterServices(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> descriptors);
}
