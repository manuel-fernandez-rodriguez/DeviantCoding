using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.SelfRegistration.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviantCoding.Registerly.UnitTests;

public class AutoRegisterServicesTest
{
    private readonly IHostApplicationBuilder _host = Host.CreateEmptyApplicationBuilder(new());

    [Fact]
    public void AutoRegisterServicesFromDefaultDependencyContext()
    {
        _host.AutoRegisterServices();
        
        Assert(_host.Services);
    }

    [Fact]
    public void AutoRegisterServicesFromAssemblies()
    {
        _host.AutoRegisterServices([GetType().Assembly]);

        Assert(_host.Services);
    }

    [Fact]
    public void ResolveServices()
    {
        _host.AutoRegisterServices();

        var services = _host.Services.BuildServiceProvider();

        services.GetRequiredService<IScopedService>()
            .Should().BeOfType<TestScopedService>();

        services.GetRequiredService<ITransientService>()
            .Should().BeOfType<TestTransientService>();

        services.GetRequiredService<ISingletonService>()
            .Should().BeOfType<TestSingletonService>();
    }


    private static void Assert(IServiceCollection services)
    {
        services.Should().HaveService<IScopedService>()
            .WithImplementation<TestScopedService>()
            .WithLifetime(ServiceLifetime.Scoped);

        services.Should().HaveService<ITransientService>()
            .WithImplementation<TestTransientService>()
            .WithLifetime(ServiceLifetime.Transient);

        services.Should().HaveService<ISingletonService>()
            .WithImplementation<TestSingletonService>()
            .WithLifetime(ServiceLifetime.Singleton);
    }

    [Singleton<AsSelf>]
    public class TestServiceDependency { }

    public interface IScopedService { }

    [Scoped]
    public class TestScopedService(TestServiceDependency serviceDependency) : IScopedService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }

    public interface ITransientService { }

    [Transient]
    public class TestTransientService(TestServiceDependency serviceDependency) : ITransientService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }

    public interface ISingletonService { }

    [Singleton]
    public class TestSingletonService(TestServiceDependency serviceDependency) : ISingletonService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }
}