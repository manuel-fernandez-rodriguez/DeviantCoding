using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies
{
    public interface ILifetimeStrategy
    {
        ServiceLifetime Map(Type implementationType);
    }
}
