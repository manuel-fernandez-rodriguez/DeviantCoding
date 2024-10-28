using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
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

    public static IServiceCollection AutoRegisterServices(this IServiceCollection services)
    {
        return services
            .FromDependencyContext()
            .AutoRegisterServices();
    }


    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.AutoRegisterServices(assemblies);
        return app;
    }

    public static IServiceCollection AutoRegisterServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services
            .FromAssemblies(assemblies)
            .AutoRegisterServices();
    }

    private static IServiceCollection AutoRegisterServices(this IClassSourceResult classes)
    {
        return classes
            .Where(t => t.IsMarkedForAutoRegistration())
            .Using<AttributeLifetimeStrategy, AttributeMappingStrategy, AttributeRegistrationStrategy>()
            .RegisterServices();
    }
}