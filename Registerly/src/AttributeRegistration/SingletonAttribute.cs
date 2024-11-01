using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.AttributeRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : RegisterlyAttribute(new Singleton(), null, null);

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy>() : RegisterlyAttribute(new Singleton(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Singleton(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();
