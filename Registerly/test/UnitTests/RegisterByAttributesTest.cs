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


    private static void Assert(IServiceCollection services)
    {
        //services.Should().HaveAtLeastOne<IScopedService>()
        //    .WithImplementation<TestScopedService>()
        //    .WithLifetime(ServiceLifetime.Scoped);

        //services.Should().HaveSingle<ITransientService>()
        //    .WithImplementation<TestTransientService>()
        //    .WithLifetime(ServiceLifetime.Transient);

        //services.Should().HaveSingle<ISingletonService>()
        //    .WithImplementation<TestSingletonService>()
        //    .WithLifetime(ServiceLifetime.Singleton);

        services.Should()
            .ContainSingle(s => s.Exactly<IScopedService, TestScopedService>(ServiceLifetime.Scoped))
            .And.ContainSingle(s => s.Exactly<ITransientService, TestTransientService>(ServiceLifetime.Transient))
            .And.ContainSingle(s => s.Exactly<ISingletonService, TestSingletonService>(ServiceLifetime.Singleton))
            ;

    }

    
}