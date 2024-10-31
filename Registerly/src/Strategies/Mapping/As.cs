using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class As<T>() : As(typeof(T))
    { }


    public class As(Type serviceType) : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
        {
            foreach(var implementationType in implementationTypes)
            {
                yield return new ServiceDescriptor(serviceType, implementationType, lifetimeStrategy.Map(implementationType));
            }
        }
    }
}
