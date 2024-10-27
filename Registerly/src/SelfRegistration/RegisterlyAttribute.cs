using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute<TLifetimeStrategy, TMappingStrategy, TRegistrationStrategy>() 
    : RegisterlyAttribute(new TLifetimeStrategy(), new TMappingStrategy(), new TRegistrationStrategy())
    where TLifetimeStrategy : ILifetimeStrategy, new()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new()
{
    public new TMappingStrategy MappingStrategy => (TMappingStrategy) base.MappingStrategy ;

}

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute : Attribute
{
    public ILifetimeStrategy LifetimeStrategy { get; }

    public IMappingStrategy MappingStrategy { get; }

    public IRegistrationStrategy RegistrationStrategy { get; }

    public RegisterlyAttribute(ILifetimeStrategy lifetimeStrategy, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
    {
        LifetimeStrategy = lifetimeStrategy;
        MappingStrategy = mappingStrategy;
        RegistrationStrategy = registrationStrategy;
    }
}

