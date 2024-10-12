using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.SelfRegistration.Strategies
{
    public class AsSelf : IRegistrationStrategy
    {
        public IServiceCollection RegisterServices(IServiceCollection serviceCollection, Type implementation, ServiceLifetime serviceLifetime)
        {
            var descriptor = new ServiceDescriptor(implementation, implementation, serviceLifetime);

            serviceCollection.Add(descriptor);

            return serviceCollection;
        }
    }
}
