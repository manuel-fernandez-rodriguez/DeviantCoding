using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.SelfRegistration.Strategies;
using DeviantCoding.Registerly.SelfRegistration.Scanning;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.UnitTests
{
    public class TypeScannerTests
    {
        [Scoped] public class TypeScannerClass1 { }
        [Singleton] public class TypeScannerClass2 { }
        [Transient<AsSelf>] public class TypeScannerClass3 { }
        [Singleton<AsSelf>] public class TypeScannerClass4 { }


        private static Type[] DecoratedTypes = 
            [
            typeof(TypeScannerClass1), typeof(TypeScannerClass2), typeof(TypeScannerClass3), 
            typeof(TypeScannerClass4)
            ];

        [Fact]
        public void FromDependencyContext()
        {
            var types = TypeSelector
                .FromDependencyContext()
                .Where( t => t.Name.StartsWith("TypeScannerClass"));

            types.Should().OnlyContain(t => DecoratedTypes.Contains(t));
        }

        [Fact]
        public void FromAssemblyNames()
        {
            var types = TypeSelector
                .FromAssemblyNames([typeof(TypeScannerTests).Assembly.GetName()], _ => true)
                .Where(t => t.Name.StartsWith("TypeScannerClass"));

            types.Should().OnlyContain(t => DecoratedTypes.Contains(t));
        }

        [Fact]
        public void ShouldResolveAsSelfAttribute()
        {
            foreach (var type in DecoratedTypes)
            {
                type.IsMarkedForAutoRegistration()
                    .Should().BeTrue("because {0} name is expected to be autoregistrable", type.Name);
                type.IsDefined(typeof(RegisterAttribute), true)
                    .Should().BeTrue("because {0} name is expected to be autoregistrable", type.Name);

            }
        }
    }
}
