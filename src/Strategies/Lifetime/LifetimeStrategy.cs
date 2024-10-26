using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Lifetime
{

    public class Scoped() : LifetimeStrategy(ServiceLifetime.Scoped) { }
    public class Singleton() : LifetimeStrategy(ServiceLifetime.Singleton) { }
    public class Transient() : LifetimeStrategy(ServiceLifetime.Transient) { }
    
    public abstract class LifetimeStrategy(ServiceLifetime lifetime) : ILifetimeStrategy
    {
        public ServiceLifetime Lifetime { get; } = lifetime;

    }
}
