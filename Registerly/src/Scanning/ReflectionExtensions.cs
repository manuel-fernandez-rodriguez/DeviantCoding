using DeviantCoding.Registerly.SelfRegistration;
using System.Reflection;

namespace DeviantCoding.Registerly.Scanning
{
    internal static class ReflectionExtensions
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

        internal static RegisterlyAttribute? GetAutoRegistrationAttribute(this Type type)
        {
            if (!type.IsRegistrable())
            {
                return null;
            }

            var allCustomAttributes = type
                    .GetCustomAttributes(true);

            foreach (var attribute in allCustomAttributes)
            {
                var attributeType = attribute.GetType();
                var t = attributeType;
                while (t != null)
                {
                    if (t == typeof(RegisterlyAttribute))
                    {
                        return (RegisterlyAttribute)attribute;
                    }
                    t = t.BaseType;
                }
            }
            return null;
        }
    }
}
