using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Registration;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : ScopedAttribute<AsImplementedInterfaces>();

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy>() : ScopedAttribute<TMappingStrategy, AddRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute<Scoped, TMappingStrategy, TRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();


