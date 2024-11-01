using System.Reflection;
using DeviantCoding.Registerly.AttributeRegistration;

namespace DeviantCoding.Registerly.Scanning;

internal static class ScanningExtensions
{
    internal static IReadOnlyCollection<Type> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null).ToArray()!;
        }
        catch
        {
            return [];
        }
    }

    internal static bool IsRegistrable(this Type type) => type.IsNonAbstractClass(publicOnly: false);

    internal static bool IsMarkedForAutoRegistration(this Type type) => type.IsDefined(typeof(RegisterlyAttribute), true);

    internal static RegisterlyAttribute? GetFirstRegisterlyAttributeOrDefault(this Type type)
    {
        return type.IsRegistrable()
            ? type.GetCustomAttributes(true)
                  .OfType<RegisterlyAttribute>()
                  .FirstOrDefault()
            : null;
    }
}
