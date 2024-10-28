using DeviantCoding.Registerly.SelfRegistration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Lifetime
{

    public class Scoped() : LifetimeStrategy(ServiceLifetime.Scoped) { }
    public class Singleton() : LifetimeStrategy(ServiceLifetime.Singleton) { }
    public class Transient() : LifetimeStrategy(ServiceLifetime.Transient) { }
    public class AttributeLifetimeStrategy : ILifetimeStrategy
    {
        public ServiceLifetime Map(Type implementationType)
        {
            return implementationType
                .GetCustomAttribute<RegisterlyAttribute>()?.
                LifetimeStrategy?.Map(implementationType)
                ?? ServiceLifetime.Scoped;
        }
    }

    public abstract class LifetimeStrategy(ServiceLifetime lifetime) : ILifetimeStrategy
    {
        public ServiceLifetime Map(Type implementationType)
        {
            return lifetime;
        }
    }
}
