using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IClassSource FromAssemblies(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        return app.Services.FromAssemblies(assemblies);
    }

    public static IClassSource FromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return new RegistrationBuilder(services, () => TypeSelector.FromAssemblies(assemblies));
    }

    public static IClassSource FromAssemblyOf<T>(this IHostApplicationBuilder app)
    {
        return app.Services.FromAssemblyOf<T>();
    }

    public static IClassSource FromAssemblyOf<T>(this IServiceCollection services)
    {
        return new RegistrationBuilder(services, () => TypeSelector.FromAssemblies([typeof(T).Assembly]));
    }
}