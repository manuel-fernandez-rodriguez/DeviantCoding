using DeviantCoding.Registerly.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviantCoding.Registerly;

public static class RegistrationExtensions
{
    public static IHostApplicationBuilder RegisterServicesByAttributes(this IHostApplicationBuilder app, Func<IClassSource, IRegistrationTaskSource> classes)
    {
        _ = app.Services.Register(classes);
        return app;
    }

    public static IHostApplicationBuilder Register(this IHostApplicationBuilder app, Func<IClassSource, IRegistrationTaskSource> classes)
    {
        app.Services.Register(classes(new RegistrationTaskBuilder()));
        return app;
    }

    public static IServiceCollection Register(this IServiceCollection services, Func<IClassSource, IRegistrationTaskSource> classes) 
        => services.Register(classes(new RegistrationTaskBuilder()));

    public static IServiceCollection Register(this IServiceCollection services, IRegistrationTaskSource source)
    {
        foreach (var task in source)
        {
            task.RegisterIn(services);
        }

        return services;
    }
}