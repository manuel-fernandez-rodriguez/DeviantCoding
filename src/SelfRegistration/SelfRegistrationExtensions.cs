using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.SelfRegistration.Scanning;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class SelfRegistrationExtensions
{
    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app)
    {
        _ = app.Services.AutoRegisterServices();
        return app;
    }

    private static IServiceCollection AutoRegisterServices(this IServiceCollection serviceCollection)
    {
        var implementationsToRegister = new AutoRegisterTypeSelector().FromDependencyContext();

        foreach (var implementation in implementationsToRegister)
        {
            var attribute = implementation.GetCustomAttribute<RegisterAsAttribute>()!;
            var (strategy, lifetime) = (attribute.RegistrationStrategy, attribute.ServiceLifetime);
            strategy.RegisterServices(serviceCollection, implementation, lifetime);
        }

        return serviceCollection;
    }

}