using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;


[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : RegisterlyAttribute<AsImplementedInterfaces>(ServiceLifetime.Transient);

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy>() : RegisterlyAttribute<TMappingStrategy>(ServiceLifetime.Transient)
    where TMappingStrategy : IMappingStrategy, new();

