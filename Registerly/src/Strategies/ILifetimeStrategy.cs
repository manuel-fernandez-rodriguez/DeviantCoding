namespace DeviantCoding.Registerly.Strategies
{
    using Microsoft.Extensions.DependencyInjection;

    public interface ILifetimeStrategy
    {
        ServiceLifetime Map(Type implementationType);
    }
}
