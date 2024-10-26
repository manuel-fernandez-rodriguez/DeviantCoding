using DeviantCoding.Registerly.UnitTests.SampleServices;
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
        services.Should().HaveSingleService<IScopedService>()
            .WithImplementation<TestScopedService>()
            .WithLifetime(ServiceLifetime.Scoped);

        services.Should().HaveSingleService<ITransientService>()
            .WithImplementation<TestTransientService>()
            .WithLifetime(ServiceLifetime.Transient);

        services.Should().HaveSingleService<ISingletonService>()
            .WithImplementation<TestSingletonService>()
            .WithLifetime(ServiceLifetime.Singleton);
    }

    
}