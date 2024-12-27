using DeviantCoding.Registerly.AttributeRegistration;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.UnitTests;
public class OpenGenericsAsSpecifiedInterfaceTest
{
    private readonly ServiceCollection _services = new();

    public interface IOpenGenericService<T> { }

    public class AsIOpenGenericService() : As(typeof(IOpenGenericService<>)) { }

    [Scoped<AsIOpenGenericService>]
    public class OpenGenericService<T> : IOpenGenericService<T> { }

    [Fact]
    public void ShouldRegisterOpenGenericsAsSpecifiedInterface()
    {
        _services.Register(classes => classes
            .FromAssemblyOf<OpenGenericsAsSpecifiedInterfaceTest>()
            .Where(t => t == typeof(OpenGenericService<>))
            .Using(new Scoped(), new As(typeof(IOpenGenericService<>)))
            );

        var provider = _services.BuildServiceProvider();

        provider.GetRequiredService<IOpenGenericService<int>>()
            .Should().BeOfType(typeof(OpenGenericService<int>));
    }

    [Fact]
    public void ShouldRegisterOpenGenericAsSpecifiedInterfaceByAttributes()
    {
        _services.RegisterByAttributes(typeof(OpenGenericService<>).Assembly);

        var provider = _services.BuildServiceProvider();

        provider.GetRequiredService<IOpenGenericService<int>>()
            .Should().BeOfType(typeof(OpenGenericService<int>));
    }

}
