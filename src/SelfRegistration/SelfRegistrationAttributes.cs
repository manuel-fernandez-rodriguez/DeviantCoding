using DeviantCoding.Registerly.SelfRegistration.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Transient);

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Transient)
    where TRegistrationStrategy : IRegistrationStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Scoped);

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Scoped)
    where TRegistrationStrategy : IRegistrationStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : RegisterAttribute<AsImplementedInterfaces>(ServiceLifetime.Singleton);

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TRegistrationStrategy>() : RegisterAttribute<TRegistrationStrategy>(ServiceLifetime.Singleton)
    where TRegistrationStrategy : IRegistrationStrategy, new();


[AttributeUsage(AttributeTargets.Class)]
public class RegisterAttribute<TRegistrationStrategy>(ServiceLifetime serviceLifetime) 
    : RegisterAttribute(serviceLifetime, new TRegistrationStrategy())
where TRegistrationStrategy : IRegistrationStrategy, new()
{
    public new TRegistrationStrategy RegistrationStrategy => (TRegistrationStrategy) base.RegistrationStrategy ;

}

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }

    public IRegistrationStrategy RegistrationStrategy { get; }

    public RegisterAttribute(ServiceLifetime serviceLifetime, IRegistrationStrategy registrationStrategy)
    {
        ServiceLifetime = serviceLifetime;
        RegistrationStrategy = registrationStrategy;
    }
}

