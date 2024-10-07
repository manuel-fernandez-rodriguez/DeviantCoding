using DeviantCoding.Registerly.SelfRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviantCoding.Registerly.UnitTests;

public class AutoRegisterServicesTest
{
    private readonly IHostApplicationBuilder _host = Host.CreateEmptyApplicationBuilder(new());
    [Fact]
    public void RegisterServiceAsScoped()
    {
        _host.AutoRegisterServices();

        _host.Services.Should().HaveService<IScopedService>()
            .WithImplementation<TestScopedService>()
            .WithLifetime(ServiceLifetime.Scoped);

        _host.Services.Should().HaveService<ITransientService>()
            .WithImplementation<TestTransientService>()
            .WithLifetime(ServiceLifetime.Transient);

        _host.Services.Should().HaveService<ISingletonService>()
            .WithImplementation<TestSingletonService>()
            .WithLifetime(ServiceLifetime.Singleton);
    }

    public class TestServiceDependency { }
    public interface IScopedService { }

    [RegisterAsScoped]
    public class TestScopedService(TestServiceDependency serviceDependency) : IScopedService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }

    public interface ITransientService { }

    [RegisterAsTransient]
    public class TestTransientService(TestServiceDependency serviceDependency) : ITransientService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }

    public interface ISingletonService { }

    [RegisterAsSingleton]
    public class TestSingletonService(TestServiceDependency serviceDependency) : ISingletonService
    {
        private readonly TestServiceDependency _serviceDependency = serviceDependency;
    }
}