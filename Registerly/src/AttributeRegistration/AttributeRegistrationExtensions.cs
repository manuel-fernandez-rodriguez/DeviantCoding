using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AttributeRegistrationExtensions
{
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app)
    {
        _ = app.Services.RegisterByAttributes();
        return app;
    }

    public static IServiceCollection RegisterByAttributes(this IServiceCollection services)
    {
        return services
            .FromDependencyContext()
            .RegisterByAttributes();
    }


    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.RegisterByAttributes(assemblies);
        return app;
    }

    public static IServiceCollection RegisterByAttributes(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services
            .FromAssemblies(assemblies)
            .RegisterByAttributes();
    }

    private static IServiceCollection RegisterByAttributes(this IClassSourceResult classes)
    {
        return classes
            .Where(t => t.IsMarkedForAutoRegistration())
            .Using<AttributeLifetimeStrategy, AttributeMappingStrategy, AttributeRegistrationStrategy>()
            .RegisterServices();
    }
}