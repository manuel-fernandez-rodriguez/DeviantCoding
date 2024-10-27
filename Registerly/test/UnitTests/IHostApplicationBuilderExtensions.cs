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

namespace Microsoft.Extensions.Hosting;

internal static class IHostApplicationBuilderExtensions
{
    public static ServiceCollectionDescriptorAssertion HaveSomeServiceImplementing<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        var implementations = target.Subject.Where(target => target.ServiceType == typeof(TService));
        return implementations.Should().NotBeEmpty().And;
    }

    public static ServiceDescriptorAssertionResult HaveSingleService<TService>(this ServiceCollectionDescriptorAssertion target)
    {
        return target.Contain(s => s.ServiceType == typeof(TService));
    }

    public static ServiceDescriptorAssertionResult WithLifetime(this ServiceDescriptorAssertionResult result, ServiceLifetime lifetime)
    {
        result.Which.Lifetime.Should().Be(lifetime);
        return result;
    }

    public static ServiceDescriptorAssertionResult WithImplementation<TImplementation>(this ServiceDescriptorAssertionResult result)
    {
        result.Which.ImplementationType.Should().BeAssignableTo<TImplementation>();
        return result;
    }

    public static ServiceDescriptorAssertionResult HaveService<TService, TImplementation>(this ServiceCollectionDescriptorAssertion target)
    {
        return target
            .HaveSingleService<TService>()
            .WithImplementation<TImplementation>();
    }

    public static ServiceDescriptorAssertionResult HaveSingleRegistrationFor<TService, TImplementation>(this ServiceCollectionDescriptorAssertion target, ServiceLifetime lifetime)
    {
        return target
            .HaveSingleService<TService>()
            .WithImplementation<TImplementation>()
            .WithLifetime(lifetime);
    }

}