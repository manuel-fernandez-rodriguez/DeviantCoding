using System.Reflection;
using DeviantCoding.Registerly;
using Microsoft.Extensions.Hosting;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class AttributeRegistrationExtensions
{
    /// <summary>
    /// Search all the accessible assemblies for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the DI container.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    /// <example>
    /// host.RegisterServicesByAttributes();
    /// </example>
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app)
    {
        _ = app.Services.RegisterByAttributes();
        return app;
    }

    /// <summary>
    /// Uses <paramref name="classes"/> source to search for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the DI container.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <param name="classes">Expression that selects the classes to register</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    /// <example>
    /// host.RegisterServicesByAttributes(classes => classes.FromAssemblyOf<TestScopedService>());
    /// </example>
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, Func<IClassSource, IClassSourceResult> classes)
    {
        _ = app.Services.Register(c => classes(c).UsingAttributes());
        return app;
    }

    /// <summary>
    /// Search all the accessible assemblies for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the target service collection.
    /// </summary>
    /// <param name="services">Target where services should be registered in</param>
    /// <returns>Tbe same <see cref="IServiceCollection"/> it was invoked on</returns>
    /// <example>
    /// host.Services.RegisterByAttributes()
    /// </example>
    public static IServiceCollection RegisterByAttributes(this IServiceCollection services)
        => services.Register(classes => classes.UsingAttributes());

    /// <summary>
    /// Search all the assemblies given in <paramref name="assemblies"/> for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the target service collection.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <param name="assemblies">List of <see cref="Assembly"/> to search in.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, params Assembly[] assemblies)
        => app.RegisterServicesByAttributes(assemblies.AsEnumerable());

    /// <summary>
    /// Search all the assemblies given in <paramref name="assemblies"/> for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the target service collection.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <param name="assemblies">List of <see cref="Assembly"/> to search in.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.RegisterByAttributes(assemblies);
        return app;
    }

    /// <summary>
    /// Search all the assemblies given in <paramref name="assemblies"/> for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the target service collection.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <param name="assemblies">List of <see cref="Assembly"/> to search in.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    public static IServiceCollection RegisterByAttributes(this IServiceCollection services, params Assembly[] assemblies)
        => services.RegisterByAttributes(assemblies.AsEnumerable());

    /// <summary>
    /// Search all the assemblies given in <paramref name="assemblies"/> for classes marked by 
    /// <see cref="DeviantCoding.Registerly.AttributeRegistration.RegisterlyAttribute"/> derived attributes
    /// and registers them in the target service collection.
    /// </summary>
    /// <param name="app"><see cref="IHostApplicationBuilder"/> to register classes in</param>
    /// <param name="assemblies">List of <see cref="Assembly"/> to search in.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> it was invoked on</returns>
    public static IServiceCollection RegisterByAttributes(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        => services
            .Register(classes => classes
                .FromAssemblies(assemblies)
                .UsingAttributes());
}