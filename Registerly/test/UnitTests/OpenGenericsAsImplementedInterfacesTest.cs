using DeviantCoding.Registerly.AttributeRegistration;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.UnitTests;
public class OpenGenericsAsImplementedInterfacesTest
{
    private readonly ServiceCollection _services = new();

    public interface IOpenGenericService<T> { }

    [Scoped]
    public class OpenGenericService<T> : IOpenGenericService<T> { }

    [Fact]
    public void ShouldRegisterOpenGenericsAsImplementedInterfaces()
    {
        _services.Register(classes => classes
            .FromAssemblyOf<OpenGenericsAsImplementedInterfacesTest>()
            .Where(t => t  == typeof(OpenGenericService<>))
            );

        var provider = _services.BuildServiceProvider();

        provider.GetRequiredService<IOpenGenericService<int>>()
            .Should().BeOfType(typeof(OpenGenericService<int>));
    }

    [Fact]
    public void ShouldRegisterOpenGenericAsImplementedInterfacesByAttributes()
    {
        _services.RegisterByAttributes(typeof(OpenGenericService<>).Assembly);

        var provider = _services.BuildServiceProvider();

        provider.GetRequiredService<IOpenGenericService<int>>()
            .Should().BeOfType(typeof(OpenGenericService<int>));
    }
}
