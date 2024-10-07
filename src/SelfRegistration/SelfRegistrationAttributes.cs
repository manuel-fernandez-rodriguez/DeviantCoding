using DeviantCoding.Registerly.SelfRegistration.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAsTransientAttribute() : RegisterAsAttribute(ServiceLifetime.Transient);

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAsScopedAttribute() : RegisterAsAttribute(ServiceLifetime.Scoped);

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAsSingletonAttribute() : RegisterAsAttribute(ServiceLifetime.Singleton);


[AttributeUsage(AttributeTargets.Class)]
public class  RegisterAsAttribute(ServiceLifetime serviceLifetime) : RegisterAsAttribute<AsImplementedInterfaces>(serviceLifetime)
{
    
}

[AttributeUsage(AttributeTargets.Class)]
public class RegisterAsAttribute<TRegistrationStrategy> : Attribute
    where TRegistrationStrategy : IRegistrationStrategy<TRegistrationStrategy>
{
    public ServiceLifetime ServiceLifetime { get; }

    public TRegistrationStrategy RegistrationStrategy { get; } = TRegistrationStrategy.Instance;

    public RegisterAsAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }
}
