using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration.Strategies
{
    public interface IRegistrationStrategy
    { 
        IServiceCollection RegisterServices(IServiceCollection serviceCollection, Type implementationType, ServiceLifetime serviceLifetime);
    }
}