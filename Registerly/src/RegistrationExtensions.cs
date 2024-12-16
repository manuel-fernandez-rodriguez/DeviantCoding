using DeviantCoding.Registerly.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviantCoding.Registerly;

/// <summary>
/// Provides extension methods for registering services in the dependency injection container.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    /// Registers classes in the host application builder.
    /// </summary>
    /// <param name="app">The host application builder.</param>
    /// <param name="classes">A function that defines the classes to register.</param>
    /// <returns>The host application builder.</returns>
    /// <example>
    /// <code>
    /// var builder = Host.CreateApplicationBuilder();
    /// builder.Register(source => source.FromAssemblyOf&lt;MyClass&gt;());
    /// </code>
    /// </example>
    public static IHostApplicationBuilder Register(this IHostApplicationBuilder app, Func<IClassSource, IRegistrationTaskSource> classes)
    {
        app.Services.Register(classes(new RegistrationTaskBuilder()));
        return app;
    }

    /// <summary>
    /// Registers classes in the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="classes">A function that defines the classes to register.</param>
    /// <returns>The service collection.</returns>
    /// <example>
    /// <code>
    /// var services = new ServiceCollection();
    /// services.Register(source => source.FromAssemblyOf&lt;MyClass&gt;());
    /// </code>
    /// </example>
    public static IServiceCollection Register(this IServiceCollection services, Func<IClassSource, IRegistrationTaskSource> classes)
        => services.Register(classes(new RegistrationTaskBuilder()));

    /// <summary>
    /// Registers classes in the service collection from a registration task source.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="source">The registration task source.</param>
    /// <returns>The service collection.</returns>
    /// <example>
    /// <code>
    /// var services = new ServiceCollection();
    /// var source = new RegistrationTaskBuilder().FromAssemblyOf&lt;MyClass&gt;();
    /// services.Register(source);
    /// </code>
    /// </example>
    public static IServiceCollection Register(this IServiceCollection services, IRegistrationTaskSource source)
    {
        foreach (var task in source)
        {
            task.RegisterIn(services);
        }

        return services;
    }
}