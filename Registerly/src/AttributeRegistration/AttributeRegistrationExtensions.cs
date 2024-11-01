using System.Reflection;
using DeviantCoding.Registerly;
using Microsoft.Extensions.Hosting;

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
        => services.Register(classes => classes.UsingAttributes());

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
        => services
            .Register(classes => classes
                .FromAssemblies(assemblies)
                .UsingAttributes());
}