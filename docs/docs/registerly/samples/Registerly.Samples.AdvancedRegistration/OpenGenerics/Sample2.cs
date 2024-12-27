using DeviantCoding.Registerly.AttributeRegistration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.OpenGenerics;
internal static class Sample2
{
    public interface IOpenGenericService<T> { }

    [Scoped]
    public class OpenGenericService<T> : IOpenGenericService<T> { }

    public static void Run()
    {
        var services = Host.CreateEmptyApplicationBuilder(new())
            .RegisterServicesByAttributes(Assembly.GetExecutingAssembly())
            .Services.BuildServiceProvider();

        var implementation = services.GetRequiredService<IOpenGenericService<int>>();
        var implementationType = implementation.GetType();

        Console.WriteLine(implementationType.Name);
        Console.WriteLine(implementationType.GetGenericArguments().First().Name);
    }
}