using DeviantCoding.Registerly.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IClassSourceResult FromAssemblies(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        return app.Services.FromAssemblies(assemblies);
    }

    public static IClassSourceResult FromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return new RegistrationBuilder(services).FromAssemblies(assemblies);
    }

    public static IClassSourceResult FromAssemblyOf<T>(this IHostApplicationBuilder app)
    {
        return app.Services.FromAssemblyOf<T>();
    }

    public static IClassSourceResult FromAssemblyOf<T>(this IServiceCollection services)
    {
        return new RegistrationBuilder(services).FromAssemblies([typeof(T).Assembly]);
    }

    public static IClassSourceResult FromAssembliesOf(this IHostApplicationBuilder app, params Type[] types)
    {
        return app.Services.FromAssembliesOf(types);
    }

    public static IClassSourceResult FromAssembliesOf(this IServiceCollection services, params Type[] types)
    {
        return new RegistrationBuilder(services).FromAssemblies(types.Select(t => t.Assembly));
    }

    public static IClassSourceResult FromClasses(this IHostApplicationBuilder app, IEnumerable<Type> candidates)
    {
        return app.Services.FromClasses(candidates);
    }

    public static IClassSourceResult FromClasses(this IServiceCollection services, IEnumerable<Type> candidates)
    {
        return new RegistrationBuilder(services).FromClasses(candidates);
    }

    public static IClassSourceResult FromDependencyContext(this IHostApplicationBuilder app)
    {
        return app.Services.FromDependencyContext();
    }

    public static IClassSourceResult FromDependencyContext(this IServiceCollection services)
    {
        return new RegistrationBuilder(services).FromDependencyContext();
    }
}