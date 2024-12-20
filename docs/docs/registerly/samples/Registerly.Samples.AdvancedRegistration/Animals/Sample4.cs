using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Strategies.Lifetime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.Animals;

internal static class Sample4
{
    public static void Run()
    {
        var services = Host.CreateEmptyApplicationBuilder(new())
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(c => c.AssignableTo<IAnimal>())
                .WithLifetime(ServiceLifetime.Transient)
                .WithFactory( s => (IAnimal) (Random.Shared.Next(0,4) switch
                {
                    0 => new Cat(),
                    1 => new Dog(),
                    2 => new Spider(),
                    3 => new Alligator(),
                    _ => throw new NotImplementedException()
                }))
            ).Services.BuildServiceProvider();

        for (int i=0; i < 5; i++)
        {
            Console.WriteLine(services.GetRequiredService<IAnimal>().Name);
        }
    }
}
