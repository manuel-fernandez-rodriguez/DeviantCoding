﻿using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IServiceCollection Register(this IServiceCollection services, Func<IClassSource, IRegistrationTaskSource> classes)
    {
        var tasks = classes(new RegistrationTaskBuilder());
        return services.Register(tasks);
    }


    public static IClassSourceResult ToSource(this IEnumerable<Type> types)
    {
        return new RegistrationTaskBuilder().From(types);
    }

    public static IServiceCollection Register(this IServiceCollection services, IRegistrationTaskSource source)
    {
        foreach (var task in source)
        {
            foreach (var candidate in task.Classes)
            {
                var (lifetime, mapping, registration) = task.GetStrategies();
                var descriptors = mapping.Map(candidate, lifetime);
                registration.RegisterServices(services, descriptors);
            }
        }

        return services;
    }
}