using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration.Mapping
{
    public interface IRegistrationStrategy<TSelf> where TSelf : IRegistrationStrategy<TSelf>
    {
        public static abstract TSelf Instance { get; }

        IServiceCollection RegisterServices(IServiceCollection serviceCollection, Type implementationType, ServiceLifetime serviceLifetime);

    }
}