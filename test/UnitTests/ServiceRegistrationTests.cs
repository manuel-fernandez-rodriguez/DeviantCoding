using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using DeviantCoding.Registerly.UnitTests.SampleServices;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization;

namespace DeviantCoding.Registerly.UnitTests;

public class ServiceRegistrationTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public interface IService1 { }
    public interface IService2 { }
    public class Implementation1 : IService1 { }
    public class Implementation2 : IService1 { }
    public class Implementation3 : IService1 { }
    public class Implementation4 : IService2 { }
    public class Implementation5 : IService2 { }

    public List<Type> Classes = [ typeof(Implementation1), typeof(Implementation2), typeof(Implementation3), typeof(Implementation4), typeof(Implementation5) ];

    [Fact]
    public void Should_aggregate_classes_lifetimes_and_scopes()
    {
        _services
            .FromAssemblyOf<Implementation1>()
            .Where(t=> t.Name == nameof(Implementation1) || t == typeof(Implementation2) || t == typeof(Implementation3))
                .WithLifetime(ServiceLifetime.Scoped)
                .WithMappingStrategy<AsSelf>()
            .AndAlso( t => t == typeof(Implementation4))
                .WithLifetime(ServiceLifetime.Singleton)
                .WithMappingStrategy<AsImplementedInterfaces>()
            .AndAlso(t => t == typeof(Implementation5))
                .WithLifetime(ServiceLifetime.Transient)
                .WithMappingStrategy<AsSelf>()
            .RegisterServices();

        VerifyServices(_services);
    }

    [Fact]
    public void Verbose_and_compact_styles_should_be_equivalent()
    {
        TestRegistration(services => services
            .FromAssemblyOf<Implementation1>()
            .Where(t => t.Name == nameof(Implementation1)|| t == typeof(Implementation2) || t == typeof(Implementation3))
                .WithLifetime(ServiceLifetime.Scoped)
                .WithMappingStrategy<AsSelf>()
            .AndAlso(t => t == typeof(Implementation4))
                .WithLifetime(ServiceLifetime.Singleton)
                .WithMappingStrategy<AsImplementedInterfaces>()
            .AndAlso(t => t == typeof(Implementation5))
                .WithLifetime(ServiceLifetime.Transient)
                .WithMappingStrategy<AsSelf>()
            .RegisterServices()
        );

        TestRegistration(services => services
            .FromAssemblyOf<Implementation1>()
            .Where(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t))
                .Using(ServiceLifetime.Scoped, MappingStrategyEnum.AsSelf)
            .AndAlso(t => t == typeof(Implementation4))
                .Using(ServiceLifetime.Singleton, MappingStrategyEnum.AsImplementedInterfaces)
            .AndAlso(t => t == typeof(Implementation5))
                .Using(ServiceLifetime.Transient, MappingStrategyEnum.AsSelf)
            .RegisterServices()
        );

        TestRegistration(services => services
            .FromAssemblies([Assembly.GetExecutingAssembly()])
            .Where(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t))
                .Using<Scoped, AsSelf>()
            .AndAlso(t => t == typeof(Implementation4))
                .Using<Singleton>()
            .AndAlso(t => t == typeof(Implementation5))
                .Using<Transient, AsSelf>()
            .RegisterServices()
        );
    }

    [Fact]
    public void Should_get_source_from_the_assembly_of_a_given_type()
    {
        TestRegistration(services => services
            .FromAssemblyOf<IService1>()
            .RegisterServices(),
            VerifyServices2
        );
    }

    [Fact]
    public void Should_get_source_from_given_classes()
    {
        TestRegistration(services => services
           .FromClasses(Classes)
           .RegisterServices(),
           VerifyServices2
       );
    }

    [Fact]
    public void Should_apply_default_lifetime()
    {
        _services.FromClasses(Classes)
            .RegisterServices();

        _services
            .Where(s => s.ServiceType == typeof(IService1) || s.ServiceType == typeof(IService2))
            .Should().OnlyContain(o => o.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void Should_chain_sources()
    {
        TestRegistration(services => services
            .FromClasses(Classes)
            .FromAssemblyOf<Exception>()
            .Where(t => t == typeof(Exception))
            .RegisterServices(),
            services => 
            {
                VerifyServices2(services);
                services.Should().HaveSingleRegistrationFor<ISerializable, Exception>(ServiceLifetime.Scoped);
            }
       );
    }
    [Fact]
    public void Meh()
    {
        var services = _services
            .FromAssemblies([Assembly.GetExecutingAssembly()])
            .Where(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t)) 
            .Using<Scoped, AsSelf>()
            .RegisterServices()
            ;

        services.Should().HaveCount(3);
        services.Should().HaveSingleRegistrationFor<Implementation1, Implementation1>(ServiceLifetime.Scoped);
        services.Should().HaveSingleRegistrationFor<Implementation2, Implementation2>(ServiceLifetime.Scoped);
        services.Should().HaveSingleRegistrationFor<Implementation3, Implementation3>(ServiceLifetime.Scoped);
    }

    private static void TestRegistration(Action<IServiceCollection> action, Action<IServiceCollection>? verify = null)
    {
        verify = verify ?? VerifyServices;

        var services = new ServiceCollection();
        action(services);
        verify(services);
    }

    private static void VerifyServices(IServiceCollection services)
    {
        services.Should().HaveSingleRegistrationFor<Implementation1, Implementation1>(ServiceLifetime.Scoped);
        services.Should().HaveSingleRegistrationFor<Implementation2, Implementation2>(ServiceLifetime.Scoped);
        services.Should().HaveSingleRegistrationFor<Implementation3, Implementation3>(ServiceLifetime.Scoped);
        services.Should().HaveSingleRegistrationFor<IService2, Implementation4>(ServiceLifetime.Singleton);
        services.Should().HaveSingleRegistrationFor<Implementation5, Implementation5>(ServiceLifetime.Transient);
    }

    private static void VerifyServices2(IServiceCollection services)
    {
        services.Where(s => s.ServiceType == typeof(IService1)).Should().HaveCount(3)
            .And.Contain(s => s.ImplementationType == typeof(Implementation1))
            .And.Contain(s => s.ImplementationType == typeof(Implementation2))
            .And.Contain(s => s.ImplementationType == typeof(Implementation3));

        services.Where(s => s.ServiceType == typeof(IService2)).Should().HaveCount(2)
            .And.Contain(s => s.ImplementationType == typeof(Implementation4))
            .And.Contain(s => s.ImplementationType == typeof(Implementation5));
    }

}

