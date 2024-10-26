using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IClassSourceResult FromAssemblies(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies, ClassFilterDelegate? predicate = null)
    {
        return app.Services.FromAssemblies(assemblies, predicate);
    }

    public static IClassSourceResult FromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ClassFilterDelegate? predicate = null)
    {
        return new RegistrationBuilder(services).FromAssemblies(assemblies, predicate);
    }

    public static IClassSourceResult FromAssemblyOf<T>(this IHostApplicationBuilder app, ClassFilterDelegate? predicate = null)
    {
        return app.Services.FromAssemblyOf<T>(predicate);
    }

    public static IClassSourceResult FromAssemblyOf<T>(this IServiceCollection services, ClassFilterDelegate? predicate = null)
    {
        return new RegistrationBuilder(services).FromAssemblies([typeof(T).Assembly], predicate);
    }

    public static IClassSourceResult FromClasses(this IHostApplicationBuilder app, IEnumerable<Type> candidates)
    {
        return app.Services.FromClasses(candidates);
    }

    public static IClassSourceResult FromClasses(this IServiceCollection services, IEnumerable<Type> candidates)
    {
        return new RegistrationBuilder(services).FromClasses(candidates);
    }

    public static IClassSourceResult FromDependencyContext(this IHostApplicationBuilder app, ClassFilterDelegate? predicate = null)
    {
        return app.Services.FromDependencyContext(predicate);
    }

    public static IClassSourceResult FromDependencyContext(this IServiceCollection services, ClassFilterDelegate? predicate = null)
    {
        return new RegistrationBuilder(services).FromDependencyContext(predicate);
    }
}