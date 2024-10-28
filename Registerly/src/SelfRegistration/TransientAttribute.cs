using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : RegisterlyAttribute(new Transient(), null, null);

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy>() : RegisterlyAttribute(new Transient(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Transient(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();
