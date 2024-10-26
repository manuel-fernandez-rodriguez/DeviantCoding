using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace DeviantCoding.Registerly.UnitTests;

public class ServiceRegistrationTest
{
    private readonly IHostApplicationBuilder _host = Host.CreateEmptyApplicationBuilder(new());

    public interface IService1 { }
    public interface IService2 { }
    public class Implementation1 : IService1 { }
    public class Implementation2 : IService1 { }
    public class Implementation3 : IService1 { }
    public class Implementation4 : IService2 { }
    public class Implementation5 : IService2 { }


    [Fact]
    public void Should_aggregate_classes_lifetimes_and_scopes()
    {
        _host.FromAssemblies([Assembly.GetExecutingAssembly()])
            .AddClasses(t => t.Name == nameof(Implementation1))
            .AddClasses(t => t == typeof(Implementation2))
            .AddClasses(t => t == typeof(Implementation3))
                .WithLifetime(ServiceLifetime.Scoped)
                .WithMappingStrategy<AsSelf>()
            .AddClasses(t => t == typeof(Implementation4))
                .WithLifetime(ServiceLifetime.Singleton)
                .WithMappingStrategy<AsImplementedInterfaces>()
            .AddClasses(t => t == typeof(Implementation5))
                .WithLifetime(ServiceLifetime.Transient)
                .WithMappingStrategy<AsSelf>()
            .Register();

        VerifyServices(_host);
    }

    [Fact]
    public void Verbose_and_compact_styles_should_be_equivalent()
    {
        Test(host => host
            .FromAssemblies([Assembly.GetExecutingAssembly()])
                .AddClasses(t => t.Name == nameof(Implementation1))
                .AddClasses(t => t == typeof(Implementation2))
                .AddClasses(t => t == typeof(Implementation3))
                    .WithLifetime(ServiceLifetime.Scoped)
                    .WithMappingStrategy<AsSelf>()
                .AddClasses(t => t == typeof(Implementation4))
                    .WithLifetime(ServiceLifetime.Singleton)
                    .WithMappingStrategy<AsImplementedInterfaces>()
                .AddClasses(t => t == typeof(Implementation5))
                    .WithLifetime(ServiceLifetime.Transient)
                    .WithMappingStrategy<AsSelf>()
                .Register()
        );

        Test(host => host
            .FromAssemblies([Assembly.GetExecutingAssembly()])
                .AddClasses(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t))
                    .Using(ServiceLifetime.Scoped, MappingStrategyEnum.AsSelf)
                .AddClasses(t => t == typeof(Implementation4))
                    .Using(ServiceLifetime.Singleton, MappingStrategyEnum.AsImplementedInterfaces)
                .AddClasses(t => t == typeof(Implementation5))
                    .Using(ServiceLifetime.Transient, MappingStrategyEnum.AsSelf)
                .Register()
        );

        Test(host => host
            .FromAssemblies([Assembly.GetExecutingAssembly()])
                .AddClasses(t => t.Name == nameof(Implementation1) || new[] { typeof(Implementation2), typeof(Implementation3) }.Contains(t))
                    .Using<Scoped, AsSelf>()
                .AddClasses(t => t == typeof(Implementation4))
                    .Using<Singleton>()
                .AddClasses(t => t == typeof(Implementation5))
                    .Using<Transient, AsSelf>()
                .Register()
        );


    }

    [Fact]
    public void Should_apply_default_strategy()
    {
        _host.FromAssemblies([Assembly.GetExecutingAssembly()])
            .AddClasses(t => t.GetInterfaces().Intersect([typeof(IService1), typeof(IService2)]).Any())
            .Register();

        _host.Services.Where(s => s.ServiceType == typeof(IService1)).Should().HaveCount(3)
            .And.Contain(s => s.ImplementationType == typeof(Implementation1))
            .And.Contain(s => s.ImplementationType == typeof(Implementation2))
            .And.Contain(s => s.ImplementationType == typeof(Implementation3));

        _host.Services.Where(s => s.ServiceType == typeof(IService2)).Should().HaveCount(2)
            .And.Contain(s => s.ImplementationType == typeof(Implementation4))
            .And.Contain(s => s.ImplementationType == typeof(Implementation5));
    }

    [Fact]
    public void Should_apply_default_lifetime()
    {
        _host.FromAssemblies([Assembly.GetExecutingAssembly()])
            .AddClasses(t => t.Name == nameof(Implementation1))
            .AddClasses(t => t == typeof(Implementation2))
            .AddClasses(t => t == typeof(Implementation3))
            .AddClasses(t => t == typeof(Implementation4))
            .AddClasses(t => t == typeof(Implementation5))
            .Register();

        _host.Services
            .Where(s => s.ServiceType == typeof(IService1) || s.ServiceType == typeof(IService2))
            .Should().OnlyContain(o => o.Lifetime == ServiceLifetime.Scoped);
    }

    private static void Test(Action<IHostApplicationBuilder> action, Action<IHostApplicationBuilder>? verify = null)
    {
        verify = verify ?? VerifyServices;

        IHostApplicationBuilder host = Host.CreateEmptyApplicationBuilder(new());
        action(host);
        verify(host);
    }

    private static void VerifyServices(IHostApplicationBuilder host)
    {
        host.Services.Should().HaveSingleRegistrationFor<Implementation1, Implementation1>(ServiceLifetime.Scoped);
        host.Services.Should().HaveSingleRegistrationFor<Implementation2, Implementation2>(ServiceLifetime.Scoped);
        host.Services.Should().HaveSingleRegistrationFor<Implementation3, Implementation3>(ServiceLifetime.Scoped);
        host.Services.Should().HaveSingleRegistrationFor<IService2, Implementation4>(ServiceLifetime.Singleton);
        host.Services.Should().HaveSingleRegistrationFor<Implementation5, Implementation5>(ServiceLifetime.Transient);
    }

}

