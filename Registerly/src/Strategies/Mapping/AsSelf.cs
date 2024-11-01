namespace DeviantCoding.Registerly.Strategies.Mapping
{
    using Microsoft.Extensions.DependencyInjection;

    public class AsSelf : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
        {
            foreach (var implementationType in implementationTypes)
            {
                yield return new ServiceDescriptor(implementationType, implementationType, lifetimeStrategy.Map(implementationType));
            }
        }
    }
}
