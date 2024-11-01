using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.AttributeRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute<TLifetimeStrategy, TMappingStrategy, TRegistrationStrategy>()
    : RegisterlyAttribute(new TLifetimeStrategy(), new TMappingStrategy(), new TRegistrationStrategy())
    where TLifetimeStrategy : ILifetimeStrategy, new()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute(ILifetimeStrategy? lifetimeStrategy, IMappingStrategy? mappingStrategy, IRegistrationStrategy? registrationStrategy) : Attribute
{
    public ILifetimeStrategy? LifetimeStrategy { get; } = lifetimeStrategy;

    public IMappingStrategy? MappingStrategy { get; } = mappingStrategy;

    public IRegistrationStrategy? RegistrationStrategy { get; } = registrationStrategy;
}