using DeviantCoding.Registerly.UnitTests.SampleServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviantCoding.Registerly.UnitTests;

public class RegisterByAttributesTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    [Fact]
    public void BasicTest()
    {
        Assert(_services.Register(classes => classes
            .FromAssemblyOf<RegisterByAttributesTest>()
            .UsingAttributes()));
    }

    [Fact]
    public void AutoRegisterServicesFromDefaultDependencyContext()
    {
        Assert(_services.RegisterByAttributes());
    }

    [Fact]
    public void AutoRegisterServicesFromAssemblies()
    {
        Assert(_services.RegisterByAttributes(GetType().Assembly));
    }

    [Fact]
    public void ResolveServices()
    {
        var host = Host.CreateEmptyApplicationBuilder(new());

        host.RegisterServicesByAttributes();

        var services = host.Services.BuildServiceProvider();

        services.GetRequiredService<IScopedService>()
            .Should().BeOfType<TestScopedService>();

        services.GetRequiredService<ITransientService>()
            .Should().BeOfType<TestTransientService>();

        services.GetRequiredService<ISingletonService>()
            .Should().BeOfType<TestSingletonService>();
    }

    [Fact]
    public void ResolveServicesFromCustomAssemblySource()
    {
        var host = Host.CreateEmptyApplicationBuilder(new());

        host.RegisterServicesByAttributes(classes => classes.FromAssemblyOf<TestScopedService>());

        var services = host.Services.BuildServiceProvider();

        services.GetRequiredService<IScopedService>()
            .Should().BeOfType<TestScopedService>();

        services.GetRequiredService<ITransientService>()
            .Should().BeOfType<TestTransientService>();

        services.GetRequiredService<ISingletonService>()
            .Should().BeOfType<TestSingletonService>();
    }

    [Fact]
    public void CanRegisterWithCustomMappingStrategy()
    {
        var host = Host.CreateEmptyApplicationBuilder(new());

        host.RegisterServicesByAttributes(classes => classes
            .FromAssemblyOf<TestScopedService>()
            .Where(c => c.Exactly<ClassImplementingTwoInterfaces>()));

        var services = host.Services.BuildServiceProvider();

        services.GetRequiredService<IInterface1>()
            .Should().BeOfType<ClassImplementingTwoInterfaces>();

        services.GetService<IInterface2>()
            .Should().BeNull();
    }


    private static void Assert(IServiceCollection services)
    {
        services.Should()
            .ContainSingle(s => s.Exactly<IScopedService, TestScopedService>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<ITransientService, TestTransientService>(ServiceLifetime.Transient))
            .And.ContainSingle(s => s.Exactly<ISingletonService, TestSingletonService>(ServiceLifetime.Singleton))
            ;
    }

    
}