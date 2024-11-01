using DeviantCoding.Registerly.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IServiceCollection Register(this IServiceCollection services, Func<IClassSource, IRegistrationTaskSource> classes)
    {
        var tasks = classes(new RegistrationTaskBuilder());
        return services.Register(tasks);
    }

    public static IServiceCollection Register(this IServiceCollection services, IRegistrationTaskSource source)
    {
        foreach (var task in source)
        {
            services.RegisterTask(task);
        }

        return services;
    }

    private static void RegisterTask(this IServiceCollection services, RegistrationTask task)
    {
        var (lifetime, mapping, registration) = task.GetStrategies();
        var descriptors = mapping.Map(task.Classes, lifetime);
        registration.RegisterServices(services, descriptors);
    }
}