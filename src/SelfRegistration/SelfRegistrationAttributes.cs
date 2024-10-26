using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Transient);

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Transient)
    where TRegistrationStrategy : IMappingStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Scoped);

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Scoped)
    where TRegistrationStrategy : IMappingStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Singleton);

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Singleton)
    where TRegistrationStrategy : IMappingStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class RegisterAttribute<TRegistrationStrategy>(ServiceLifetime serviceLifetime) 
    : RegisterAttribute(serviceLifetime, new TRegistrationStrategy())
where TRegistrationStrategy : IMappingStrategy, new()
{
    public TRegistrationStrategy RegistrationStrategy => (TRegistrationStrategy) base.MappingStrategy ;

}

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }

    public IMappingStrategy MappingStrategy { get; }

    public RegisterAttribute(ServiceLifetime serviceLifetime, IMappingStrategy mappingStrategy)
    {
        ServiceLifetime = serviceLifetime;
        MappingStrategy = mappingStrategy;
    }
}

