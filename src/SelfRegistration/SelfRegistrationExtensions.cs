using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class SelfRegistrationExtensions
{
    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app)
    {
        _ = app.Services.AutoRegisterServices(TypeSelector.FromDependencyContext(t => t.IsMarkedForAutoRegistration()));
        return app;
    }

    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.AutoRegisterServices(TypeSelector.FromAssemblies(assemblies, t => t.IsMarkedForAutoRegistration()));
        return app;
    }

    private static IServiceCollection AutoRegisterServices(this IServiceCollection serviceCollection, IEnumerable<Type> implementationsToRegister)
    {
        
        foreach (var implementation in implementationsToRegister)
        {
            var attribute = implementation.GetCustomAttribute<RegisterlyAttribute>()!;
            var (mappingStrategy, lifetime) = (attribute.MappingStrategy, attribute.ServiceLifetime);

            new RegistrationBuilder(serviceCollection).FromClasses([implementation])
                .Using(lifetime, mappingStrategy)
                .RegisterServices();
        }

        return serviceCollection;
    }

}