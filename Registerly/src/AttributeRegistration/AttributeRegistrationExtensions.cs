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
        return services.Register(classes => classes
                .FromDependencyContext()
                .UsingAttributes());
    }

    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, params Assembly[] assemblies)
        => app.RegisterServicesByAttributes(assemblies.AsEnumerable());

    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.RegisterByAttributes(assemblies);
        return app;
    }

    public static IServiceCollection RegisterByAttributes(this IServiceCollection services, params Assembly[] assemblies)
        => services.RegisterByAttributes(assemblies.AsEnumerable());

    public static IServiceCollection RegisterByAttributes(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services.Register(classes =>classes
                .FromAssemblies(assemblies)
                .UsingAttributes());
    }

}