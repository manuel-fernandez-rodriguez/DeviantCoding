using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.UnitTests;
internal static class ServiceDescriptorExtensions
{
    public static bool Exactly<TService>(this ServiceDescriptor target)
    {
        return target.ServiceType == typeof(TService);
    }

    public static bool ImplementationTypeIsExactly<TImplementationType>(this ServiceDescriptor target)
    {
        return target.ImplementationType == typeof(TImplementationType);
    }

    public static bool LifetimeIs(this ServiceDescriptor target, ServiceLifetime lifetime)
    {
        return target.Lifetime == lifetime;
    }

    public static bool Exactly<TService, TImplementationType>(this ServiceDescriptor target)
    {
        return target.Exactly<TService>() && target.ImplementationTypeIsExactly<TImplementationType>();
    }

    public static bool Exactly<TService, TImplementationType>(this ServiceDescriptor target, ServiceLifetime lifetime)
    {
        return target.Exactly<TService, TImplementationType>() && target.LifetimeIs(lifetime);
    }

}
