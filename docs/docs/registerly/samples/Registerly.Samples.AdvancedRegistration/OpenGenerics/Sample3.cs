using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.OpenGenerics;

internal static class Sample3
{
    public interface IOpenGenericService<T> { }

    public class OpenGenericService<T> : IOpenGenericService<T> { }

    public static void Run()
    {
        var services = Host.CreateEmptyApplicationBuilder(new())
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(c => c == typeof(OpenGenericService<>))
                .Using(new Scoped(), new As(typeof(IOpenGenericService<>))))
            .Services.BuildServiceProvider();

        var implementation = services.GetRequiredService<IOpenGenericService<int>>();
        var implementationType = implementation.GetType();

        Console.WriteLine(implementationType.Name);
        Console.WriteLine(implementationType.GetGenericArguments().First().Name);
    }
}