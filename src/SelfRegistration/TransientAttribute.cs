using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Registration;

namespace DeviantCoding.Registerly.SelfRegistration;


[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : TransientAttribute<AsImplementedInterfaces>();

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy>() : TransientAttribute<TMappingStrategy, AddRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute<Transient, TMappingStrategy, TRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();
