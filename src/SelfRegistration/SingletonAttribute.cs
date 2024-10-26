using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : RegisterlyAttribute<AsImplementedInterfaces>(ServiceLifetime.Singleton);

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy>() : RegisterlyAttribute<TMappingStrategy>(ServiceLifetime.Singleton)
    where TMappingStrategy : IMappingStrategy, new();
