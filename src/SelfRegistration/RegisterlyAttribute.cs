using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.SelfRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute<TMappingStrategy>(ServiceLifetime serviceLifetime) 
    : RegisterlyAttribute(serviceLifetime, new TMappingStrategy())
where TMappingStrategy : IMappingStrategy, new()
{
    public new TMappingStrategy MappingStrategy => (TMappingStrategy) base.MappingStrategy ;

}

[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }

    public IMappingStrategy MappingStrategy { get; }

    public RegisterlyAttribute(ServiceLifetime serviceLifetime, IMappingStrategy mappingStrategy)
    {
        ServiceLifetime = serviceLifetime;
        MappingStrategy = mappingStrategy;
    }
}

