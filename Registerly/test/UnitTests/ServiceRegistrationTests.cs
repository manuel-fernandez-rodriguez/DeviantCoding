using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.Serialization;

namespace DeviantCoding.Registerly.UnitTests;

public class ServiceRegistrationTest
{
    private readonly ServiceCollection _services = new();

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
            .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2), typeof(Implementation3)))
                .WithLifetime(ServiceLifetime.Scoped)
                .WithMappingStrategy<AsSelf>()
            .AndAlso(t => t.Exactly<Implementation4>())
                .WithLifetime(ServiceLifetime.Singleton)
                .WithMappingStrategy<AsImplementedInterfaces>()
            .AndAlso(t => t.Exactly<Implementation5>())
                .WithLifetime(ServiceLifetime.Transient)
                .WithMappingStrategy<AsSelf>());

        VerifyServices(_services);
    }

    [Fact]
    public void Verbose_compact_and_slim_styles_should_be_equivalent()
    {
        _services
            .Register(classes => classes
                .FromAssemblyOf<Implementation1>()
                .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2), typeof(Implementation3)))
                    .WithLifetime(ServiceLifetime.Scoped)
                    .WithMappingStrategy<AsSelf>()
                    .WithRegistrationStrategy<AddRegistrationStrategy>()
                .AndAlso(t => t == typeof(Implementation4))
                    .WithLifetime(ServiceLifetime.Singleton)
                    .WithMappingStrategy<AsImplementedInterfaces>()
                    .WithRegistrationStrategy<AddRegistrationStrategy>()
                .AndAlso(t => t == typeof(Implementation5))
                    .WithLifetime(ServiceLifetime.Transient)
                    .WithMappingStrategy<AsSelf>()
                    .WithRegistrationStrategy<AddRegistrationStrategy>()
            );

        VerifyServices(_services);
        _services.Clear();

        _services
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2), typeof(Implementation3)))
                    .Using<Scoped, AsSelf, AddRegistrationStrategy>()
                .AndAlso(t => t == typeof(Implementation4))
                    .Using<Singleton, AsImplementedInterfaces, AddRegistrationStrategy>()
                .AndAlso(t => t == typeof(Implementation5))
                    .Using<Transient, AsSelf, AddRegistrationStrategy>()
            );

        VerifyServices(_services);
        _services.Clear();

        _services
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2), typeof(Implementation3)))
                    .Using<Scoped, AsSelf>()
                .AndAlso(t => t.Exactly<Implementation4>())
                    .Using<Singleton>()
                .AndAlso(t => t.Exactly<Implementation5>())
                    .Using<Transient, AsSelf>()
            );
    }

    [Fact]
    public void Should_get_source_from_the_assembly_of_a_given_type()
    {
        _services.Register(classes => classes.FromAssemblyOf<IService1>());
            
        VerifyServices2(_services);
    }

    [Fact]
    public void Should_register_by_interface()
    {
        _services
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(t => t.AssignableTo<IService2>())
            );

        _services.Should()
            .HaveCount(2)
            .And.ContainSingle(s => s.Exactly<IService2, Implementation4>())
            .And.ContainSingle(s => s.Exactly<IService2, Implementation5>());
    }

    [Fact]
    public void Should_get_source_from_given_classes()
    {
        _services.Register(classes => classes.From(ClassList));

        VerifyServices2(_services);
    }

    [Fact]
    public void Should_apply_default_lifetime()
    {
        _services.Register(c => c.From(ClassList));

        _services
            .Where(s => s.Exactly<IService1>() || s.Exactly<IService2>())
            .Should().OnlyContain(s => s.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void Should_chain_sources()
    {
        _services
            .Register(classes => classes
                .From(ClassList)
                .FromAssemblyOf<Exception>()
                    .Where(t => t.Exactly<Exception>())
                    .WithLifetime(ServiceLifetime.Scoped)
                .AndAlso(t => t.Exactly<ArgumentNullException>())
                    .WithLifetime(ServiceLifetime.Transient)
            );

        VerifyServices2(_services);

        _services.Should()
            .ContainSingle(s => s.Exactly<ISerializable, Exception>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<ISerializable, ArgumentNullException>(ServiceLifetime.Transient));

        var serviceProvider = _services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var implementations = serviceProvider.GetRequiredService<IEnumerable<ISerializable>>();
        implementations.Should()
            .Contain(o => o.GetType().Exactly<Exception>())
            .And
            .Contain(o => o.GetType().Exactly<ArgumentNullException>());
    }

    [Fact]
    public void Should_register_as_designated_type()
    {
        _services
            .Register(classes => classes
                .From(ClassList)
                .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2)))
                .As<IService1>());

        _services.Should().HaveCount(2);
        _services.Where(s => s.ServiceType.Exactly<IService1>()).Should()
            .HaveCount(2)
            .And.Contain(s => s.Exactly<IService1, Implementation1>())
            .And.Contain(s => s.Exactly<IService1, Implementation2>());
    }

    [Fact]
    public void Should_use_default_context_when_not_specified()
    {
        _services
           .Register(classes => classes
               .Where(t => t.ExactlyAnyOf(typeof(Implementation1), typeof(Implementation2)))
               .As<IService1>());

        _services.Should().HaveCount(2);
        _services.Where(s => s.ServiceType.Exactly<IService1>()).Should()
            .HaveCount(2)
            .And.Contain(s => s.Exactly<IService1, Implementation1>())
            .And.Contain(s => s.Exactly<IService1, Implementation2>());
    }

    [Fact]
    public void Should_register_as_factory()
    {
        _services.Register(classes => classes
            .From(ClassList)
            .Where(t => t.Exactly<Implementation1>())
                .WithFactory<IService1>(s => new Implementation1 { Value = "potato1" })
            .AndAlso(t => t.Exactly<Implementation1>())
                .WithFactory(s => new Implementation1 { Value = "potato2" }));

        var serviceProvider = _services.BuildServiceProvider(); 
        using var scope = serviceProvider.CreateScope();
        serviceProvider.GetRequiredService<IService1>().Value.Should().Be("potato1");
        serviceProvider.GetRequiredService<Implementation1>().Value.Should().Be("potato2");
    }

    private static void VerifyServices(IServiceCollection services)
    {
        services.Should()
            .HaveCount(5)
            .And.ContainSingle(s => s.Exactly<Implementation1, Implementation1>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<Implementation2, Implementation2>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<Implementation3, Implementation3>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<IService2, Implementation4>(ServiceLifetime.Singleton))
            .And.ContainSingle(s => s.Exactly<Implementation5, Implementation5>(ServiceLifetime.Transient));
    }

    private static void VerifyServices2(IServiceCollection services)
    {        
        services.Where(s => s.ServiceType == typeof(IService1)).Should().HaveCount(3)
            .And.Contain(s => s.ImplementationType.Exactly<Implementation1>())
            .And.Contain(s => s.ImplementationType.Exactly<Implementation2>())
            .And.Contain(s => s.ImplementationType.Exactly<Implementation3>());

        services.Where(s => s.ServiceType == typeof(IService2)).Should().HaveCount(2)
            .And.Contain(s => s.ImplementationType.Exactly<Implementation4>())
            .And.Contain(s => s.ImplementationType.Exactly<Implementation5>());
    }

}

