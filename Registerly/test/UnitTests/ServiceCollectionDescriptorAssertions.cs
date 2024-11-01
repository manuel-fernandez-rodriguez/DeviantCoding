using ServiceCollectionDescriptorAssertion = FluentAssertions.Collections.GenericCollectionAssertions
<
    System.Collections.Generic.IEnumerable<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>, 
    Microsoft.Extensions.DependencyInjection.ServiceDescriptor, 
    FluentAssertions.Collections.GenericCollectionAssertions<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>
>;
using ServiceDescriptorAssertionResult = FluentAssertions.AndWhichConstraint
<
    FluentAssertions.Collections.GenericCollectionAssertions<Microsoft.Extensions.DependencyInjection.ServiceDescriptor>, 
    Microsoft.Extensions.DependencyInjection.ServiceDescriptor
>;

using Microsoft.Extensions.DependencyInjection;


internal static class ServiceCollectionDescriptorAssertions
{
    public static ServiceCollectionDescriptorAssertion HaveSomeServiceImplementing<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        var implementations = target.Subject.Where(target => target.ServiceType == typeof(TService));
        return implementations.Should().NotBeEmpty().And;
    }

    public static ServiceDescriptorAssertionResult HaveSingleService<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        return target.ContainSingle(s => s.ServiceType == typeof(TService));
    }

    public static ServiceDescriptorAssertionResult WithLifetime(this ServiceDescriptorAssertionResult result, ServiceLifetime lifetime)
    {
        result.Which.Lifetime.Should().Be(lifetime);
        return result;
    }

    public static ServiceDescriptorAssertionResult WithImplementation<TImplementation>(this ServiceDescriptorAssertionResult result)
    {
        result.Which.ImplementationType.Should().Be<TImplementation>();
        return result;
    }
}