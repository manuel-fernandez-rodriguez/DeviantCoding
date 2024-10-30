using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.AttributeRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : RegisterlyAttribute(new Scoped(), null, null);

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy>() : RegisterlyAttribute(new Scoped(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new ();

[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Scoped(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();
