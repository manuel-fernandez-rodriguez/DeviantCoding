using DeviantCoding.Registerly.AttributeRegistration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies.Mapping;

namespace DeviantCoding.Registerly.UnitTests;

public class TypeScannerTests
{
    public interface ITypeScannerInterface1 { }
    [Scoped] public class TypeScannerClass1 : ITypeScannerInterface1 { }
    [Singleton] public class TypeScannerClass2 : ITypeScannerInterface1 { }
    [Transient<AsSelf>] public class TypeScannerClass3 { }
    [Singleton<AsSelf>] public class TypeScannerClass4 { }


    private readonly static Type[] _decoratedTypes = 
        [
        typeof(TypeScannerClass1), typeof(TypeScannerClass2), typeof(TypeScannerClass3), 
        typeof(TypeScannerClass4)
        ];

    [Fact]
    public void Should_succesfully_execute_FromDependencyContext()
    {
        var types = TypeScanner
            .FromDependencyContext()
            .Where( t => t.Name.StartsWith("TypeScannerClass"));

        types.Should().OnlyContain(t => _decoratedTypes.Contains(t));
    }

    [Fact]
    public void Should_succesfully_execute_FromAssemblyNames()
    {
        var types = TypeScanner
            .From([typeof(TypeScannerTests).Assembly.GetName()], _ => true)
            .Where(t => t.Name.StartsWith("TypeScannerClass"));

        types.Should().OnlyContain(t => _decoratedTypes.Contains(t));
    }

    [Fact]
    public void Should_resolve_RegisterlyAttribute()
    {
        foreach (var type in _decoratedTypes)
        {
            type.IsMarkedForAutoRegistration()
                .Should().BeTrue("because {0} name is expected to be autoregistrable", type.Name);
            type.IsDefined(typeof(RegisterlyAttribute), true)
                .Should().BeTrue("because {0} name is expected to be autoregistrable", type.Name);

        }
    }

    [Fact]
    public void Should_apply_AssignableTo()
    {
        TypeScanner.From(_decoratedTypes)
            .Where(t => t.AssignableTo<ITypeScannerInterface1>())
            .Should().OnlyContain(t => new[] { typeof(TypeScannerClass1), typeof(TypeScannerClass2) }.Contains(t));

    }
}
