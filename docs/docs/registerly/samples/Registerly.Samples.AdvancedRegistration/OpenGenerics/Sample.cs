using DeviantCoding.Registerly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.OpenGenerics;
internal static class Sample
{
    public interface IOpenGenericService<T> { }

    public class OpenGenericService<T> : IOpenGenericService<T> { }

    public static void Run()
    {
        var services = Host.CreateEmptyApplicationBuilder(new())
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(c => c == typeof(OpenGenericService<>))
            ).Services.BuildServiceProvider();

        var implementation = services.GetRequiredService<IOpenGenericService<int>>();
        var implementationType = implementation.GetType();

        Console.WriteLine(implementationType.Name);
        Console.WriteLine(implementationType.GetGenericArguments().First().Name);
    }
}