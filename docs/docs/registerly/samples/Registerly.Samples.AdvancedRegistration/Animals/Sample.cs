using DeviantCoding.Registerly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.Animals;

internal static class Sample
{
    public static void Run()
    {
        var services = Host.CreateEmptyApplicationBuilder(new())
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(c => c.AssignableTo<IAnimal>())
            ).Services.BuildServiceProvider();

        var allAnimals = services.GetRequiredService<IEnumerable<IAnimal>>();
        foreach (var animal in allAnimals)
        {
            Console.WriteLine(animal.Name);
        }
    }
}
