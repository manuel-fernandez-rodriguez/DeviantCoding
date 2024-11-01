using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeviantCoding.Registerly;

public static class ReflectionExtensions
{
    public static bool IsNonAbstractClass(this Type type, bool publicOnly)
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

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
        foreach (var implementedInterface in type.GetInterfaces())
        {
            yield return implementedInterface;
        }

        var baseType = type.BaseType;
        while (baseType != null)
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }

    public static bool IsInNamespace(this Type type, string @namespace)
    {
        var typeNamespace = type.Namespace ?? string.Empty;

        if (@namespace.Length > typeNamespace.Length)
        {
            return false;
        }

        var typeSubNamespace = typeNamespace[..@namespace.Length];

        if (typeSubNamespace.Equals(@namespace, StringComparison.Ordinal))
        {
            if (typeNamespace.Length == @namespace.Length)
            {
                //exactly the same
                return true;
            }

            //is a subnamespace?
            return typeNamespace[@namespace.Length] == '.';
        }

        return false;
    }

    public static bool IsInExactNamespace(this Type type, string @namespace)
    {
        return string.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
    }

    public static bool HasAttribute(this Type type, Type attributeType)
    {
        return type.IsDefined(attributeType, inherit: true);
    }

    public static bool HasAttribute<T>(this Type type) where T : Attribute
    {
        return type.HasAttribute(typeof(T));
    }

    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
    {
        return type.GetCustomAttributes<T>(inherit: true).Any(predicate);
    }

    public static bool IsBasedOn(this Type type, Type otherType) => 
        (otherType.IsGenericTypeDefinition) ?
            type.IsAssignableToGenericTypeDefinition(otherType)
            : otherType.IsAssignableFrom(type);

    private static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericType)
    {
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType.IsGenericType)
            {
                var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                if (genericTypeDefinition == genericType)
                {
                    return true;
                }
            }
        }

        if (type.IsGenericType)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == genericType)
            {
                return true;
            }
        }

        var baseType = type.BaseType;
        if (baseType is null)
        {
            return false;
        }

        return baseType.IsAssignableToGenericTypeDefinition(genericType);
    }

    public static bool IsOpenGeneric(this Type type)
    {
        return type.IsGenericTypeDefinition;
    }

    public static bool HasMatchingGenericArity(this Type interfaceType, Type type)
    {
        if (type.IsGenericType)
        {
            if (interfaceType.IsGenericType)
            {
                if (!interfaceType.ContainsGenericParameters)
                {
                    return true;
                }

                var argumentCount = interfaceType.GetGenericArguments().Length;
                var parameterCount = type.GetGenericArguments().Length;

                return argumentCount == parameterCount;
            }

            return false;
        }

        return true;
    }

    public static bool HasCompatibleGenericArguments(this Type type, Type genericTypeDefinition)
    {
        var genericArguments = type.GetGenericArguments();
        try
        {
            _ = genericTypeDefinition.MakeGenericType(genericArguments);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public static Type GetRegistrationType(this Type interfaceType, Type type)
    {
        if (type.IsGenericTypeDefinition && interfaceType.IsGenericType)
        {
            return interfaceType.GetGenericTypeDefinition();
        }

        return interfaceType;
    }
    
    public static bool AssignableTo<T>(this Type target) => target.AssignableTo(typeof(T));

    public static bool AssignableTo(this Type target, Type type) => target.AssignableToAnyOf(type);

    public static bool AssignableToAnyOf(this Type target, params Type[] types) => target.AssignableToAnyOf(types.AsEnumerable());

    public static bool AssignableToAnyOf(this Type target, IEnumerable<Type> types) => types.Any(t => target.IsBasedOn(t));

    public static bool Exactly<T>(this Type? target) => target == typeof(T);

    public static bool ExactlyAnyOf(this Type? target, params Type[] types) => target.ExactlyAnyOf(types.AsEnumerable());

    public static bool ExactlyAnyOf(this Type? target, IEnumerable<Type> types) => types.Contains(target);



}