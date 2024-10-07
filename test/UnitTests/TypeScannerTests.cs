using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.SelfRegistration.Mapping;
using DeviantCoding.Registerly.SelfRegistration.Scanning;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DeviantCoding.Registerly.UnitTests.AutoRegisterServicesTest;

namespace DeviantCoding.Registerly.UnitTests
{
    public class TypeScannerTests
    {
        [RegisterAsScoped] public class TypeScannerClass1 { }
        [RegisterAs(ServiceLifetime.Singleton)] public class TypeScannerClass2 { }
        [RegisterAs<AsImplementedInterfaces>(ServiceLifetime.Singleton)] public class TypeScannerClass3 { }

        private static Type[] ExpectedTypes = [typeof(TypeScannerClass1), typeof(TypeScannerClass2), typeof(TypeScannerClass3)];

        [Fact]
        public void FromDependencyContext()
        {
            var scanner = new AutoRegisterTypeSelector();
            var types = scanner.FromDependencyContext()
                .Where( t => t.Name.StartsWith("TypeScannerClass"));

            types.Should().OnlyContain(t => ExpectedTypes.Contains(t));
        }

        [Fact]
        public void FromAssemblyNames()
        {
            var scanner = new AutoRegisterTypeSelector();
            var types = scanner.FromAssemblyNames([typeof(TypeScannerTests).Assembly.GetName()], _ => true, _ => true)
                .Where(t => t.Name.StartsWith("TypeScannerClass"));

            types.Should().OnlyContain(t => ExpectedTypes.Contains(t));
        }

        
    }
}
