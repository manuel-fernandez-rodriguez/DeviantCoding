using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class AsImplementedInterfaces : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
        {
            var services = implementationType.GetInterfaces();
            return services.Select(service => new ServiceDescriptor(service, implementationType, lifetimeStrategy.Map(implementationType)));
        }
    }
}
