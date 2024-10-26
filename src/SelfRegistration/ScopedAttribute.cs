using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : RegisterlyAttribute<AsImplementedInterfaces>(ServiceLifetime.Scoped);

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy>() : RegisterlyAttribute<TMappingStrategy>(ServiceLifetime.Scoped)
    where TMappingStrategy : IMappingStrategy, new();


