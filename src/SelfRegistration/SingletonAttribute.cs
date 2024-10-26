using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Registration;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : SingletonAttribute<AsImplementedInterfaces>();

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy>() : SingletonAttribute<TMappingStrategy, AddRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute<Singleton, TMappingStrategy, TRegistrationStrategy>()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();

