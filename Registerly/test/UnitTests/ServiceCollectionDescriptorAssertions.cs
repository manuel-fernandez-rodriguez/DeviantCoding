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
using DeviantCoding.Registerly.UnitTests;


internal static class ServiceCollectionDescriptorAssertions
{
    public static ServiceDescriptorAssertionResult HaveAtLeastOne<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        return target.Contain(s => s.Exactly<TService>());
    }

    public static ServiceDescriptorAssertionResult HaveSingle<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        return target.ContainSingle(s => s.Exactly<TService>());
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