using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.SelfRegistration.Scanning
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

        internal static bool IsNonAbstractClass(this Type type, bool publicOnly)
        {
            if (type.IsSpecialName)
            {
                return false;
            }

            if (type.IsClass && !type.IsAbstract)
            {
                if (type.HasAttribute<CompilerGeneratedAttribute>())
                {
                    return false;
                }

                if (publicOnly)
                {
                    return type.IsPublic || type.IsNestedPublic;
                }

                return true;
            }

            return false;
        }

        internal static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.IsDefined(attributeType, inherit: true);
        }

        internal static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.HasAttribute(typeof(T));
        }

        internal static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
        {
            return type.GetCustomAttributes<T>(inherit: true).Any(predicate);
        }
    }
}
