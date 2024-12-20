using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Registerly.Samples.AdvancedRegistration.Animals;

internal static class Sample3
{
    public static void Run()
    {
        Host.CreateEmptyApplicationBuilder(new())
            .Register(classes => classes
                .FromAssembly(Assembly.GetExecutingAssembly())
                .Where(c => c.Exactly<Cat>())
                    .Using<Transient, As<IAnimal>>()
                .AndAlso(c => c.Exactly<Dog>())
                    .Using<Singleton, AsSelf>()
                .AndAlso(c => c.ExactlyAnyOf(typeof(Spider), typeof(Alligator)))
            ).Services.BuildServiceProvider();
    }
}
