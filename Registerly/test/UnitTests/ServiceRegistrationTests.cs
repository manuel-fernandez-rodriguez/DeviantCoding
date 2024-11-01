using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.Serialization;

namespace DeviantCoding.Registerly.UnitTests;

public class ServiceRegistrationTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public interface IService1 { string Value { get; set; } }
    public interface IService2 { }
    public abstract class Service1Base : IService1 { public string Value { get; set; } = ""; }
    public class Implementation1 : Service1Base {  }
    public class Implementation2 : Service1Base { }
    public class Implementation3 : Service1Base { }
    public class Implementation4 : IService2 { }
    public class Implementation5 : IService2 { }

    public List<Type> ClassList = [ typeof(Implementation1), typeof(Implementation2), typeof(Implementation3), typeof(Implementation4), typeof(Implementation5) ];

    [Fact]
    public void Should_aggregate_classes_lifetimes_and_scopes()
    {
        _services.Register(classes => classes
            .FromAssemblyOf<Implementation1>()
            .Where(t => t.Name == nameof(Implementation1) || t == typeof(Implementation2) || t == typeof(Implementation3))
                .WithLifetime(ServiceLifetime.Scoped)
                .WithMappingStrategy<AsSelf>()
            .AndAlso(t => t == typeof(Implementation4))
                .WithLifetime(ServiceLifetime.Singleton)
                .WithMappingStrategy<AsImplementedInterfaces>()
            .AndAlso(t => t == typeof(Implementation5))
                .WithLifetime(ServiceLifetime.Transient)
                .WithMappingStrategy<AsSelf>());

        VerifyServices(_services);
    }

    [Fact]
    public void Verbose_and_compact_styles_should_be_equivalent()
    {
        TestRegistration(services => services
            .Register(classes => classes
                .FromAssemblyOf<Implementation1>()
                .Where(t => t.Name == nameof(Implementation1)|| t == typeof(Implementation2) || t == typeof(Implementation3))
                    .WithLifetime(ServiceLifetime.Scoped)
                    .WithMappingStrategy<AsSelf>()
                .AndAlso(t => t == typeof(Implementation4))
                    .WithLifetime(ServiceLifetime.Singleton)
                    .WithMappingStrategy<AsImplementedInterfaces>()
                .AndAlso(t => t == typeof(Implementation5))
                    .WithLifetime(ServiceLifetime.Transient)
                    .WithMappingStrategy<AsSelf>())
        );

        TestRegistration(services => services
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t))
                    .Using<Scoped, AsSelf>()
                .AndAlso(t => t == typeof(Implementation4))
                    .Using<Singleton>()
                .AndAlso(t => t == typeof(Implementation5))
                    .Using<Transient, AsSelf>())
        );
    }

    [Fact]
    public void Should_get_source_from_the_assembly_of_a_given_type()
    {
        TestRegistration(services => services
            .Register(classes => classes.FromAssemblyOf<IService1>()),
            VerifyServices2
        );
    }

    [Fact]
    public void Should_get_source_from_given_classes()
    {
        TestRegistration(services => services
            .Register(classes => classes.From(ClassList)),
           VerifyServices2
       );
    }

    [Fact]
    public void Should_apply_default_lifetime()
    {
        _services.Register(ClassList.AsRegistrable());

        _services
            .Where(s => s.ServiceType.IsExactly<IService1>() || s.ServiceType.IsExactly<IService2>())
            .Should().OnlyContain(o => o.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void Should_chain_sources()
    {
        TestRegistration(services => services
            .Register(ClassList.AsRegistrable()
                .FromAssemblyOf<Exception>()
                .Where(t => t.IsExactly<Exception>())),
            services => 
            {
                VerifyServices2(services);
                services.Should().HaveSingleRegistrationFor<ISerializable, Exception>(ServiceLifetime.Scoped);
            }
       );
    }

    [Fact]
    public void Should_register_as_designated_type()
    {
        var services = _services
            .Register(ClassList.AsRegistrable()
                .Where(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2) }.Contains(t)) 
                .As<IService1>());

        services.Should().HaveCount(2);
        services.Where(s => s.ServiceType.IsExactly<IService1>()).Should().HaveCount(2)
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation1>())
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation2>());
    }

    [Fact]
    public void Should_register_as_factory()
    {
        var services = _services.Register(ClassList.AsRegistrable()
                .Where(t => t.IsExactly<Implementation1>())
                    .WithFactory<IService1>(s => new Implementation1 { Value = "potato1" })
                .AndAlso(t => t.IsExactly<Implementation1>())
                    .WithFactory(s => new Implementation1 { Value = "potato2" }));

        var serviceProvider = services.BuildServiceProvider(); 
        using var scope = serviceProvider.CreateScope();
        serviceProvider.GetRequiredService<IService1>().Value.Should().Be("potato1");
        serviceProvider.GetRequiredService<Implementation1>().Value.Should().Be("potato2");
    }

    private static void TestRegistration(Action<IServiceCollection> action, Action<IServiceCollection>? verify = null)
    {
        verify ??= VerifyServices;

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
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation1>())
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation2>())
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation3>());

        services.Where(s => s.ServiceType == typeof(IService2)).Should().HaveCount(2)
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation4>())
            .And.Contain(s => s.ImplementationType.IsExactly<Implementation5>());
    }

}

