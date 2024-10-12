﻿using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.SelfRegistration.Scanning;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class SelfRegistrationExtensions
{
    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app)
    {
        _ = app.Services.AutoRegisterServices(TypeSelector.FromDependencyContext());
        return app;
    }

    public static IHostApplicationBuilder AutoRegisterServices(this IHostApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        _ = app.Services.AutoRegisterServices(TypeSelector.FromAssemblies(assemblies));
        return app;
    }

    private static IServiceCollection AutoRegisterServices(this IServiceCollection serviceCollection, IEnumerable<Type> implementationsToRegister)
    {
        
        foreach (var implementation in implementationsToRegister)
        {
            var attribute = implementation.GetCustomAttribute<RegisterAttribute>()!;
            var (strategy, lifetime) = (attribute.RegistrationStrategy, attribute.ServiceLifetime);
            strategy.RegisterServices(serviceCollection, implementation, lifetime);
        }

        return serviceCollection;
    }

}