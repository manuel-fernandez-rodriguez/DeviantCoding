﻿using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeviantCoding.Registerly;

public static class ReflectionExtensions
{
    public static bool IsNonAbstractClass(this Type type, bool publicOnly)
    {
        return !type.IsSpecialName &&
               type.IsClass &&
               !type.IsAbstract &&
               !type.HasAttribute<CompilerGeneratedAttribute>() &&
               (!publicOnly || type.IsPublic || type.IsNestedPublic);
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
        return typeNamespace.StartsWith(@namespace, StringComparison.Ordinal) &&
               (typeNamespace.Length == @namespace.Length || typeNamespace[@namespace.Length] == '.');
    }

    public static bool IsInExactNamespace(this Type type, string @namespace)
    {
        return string.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
    }

    public static bool HasAttribute(this Type type, Type attributeType)
    {
        return type.IsDefined(attributeType, inherit: true);
    }

    public static bool HasAttribute<T>(this Type type)
        where T : Attribute
    {
        return type.HasAttribute(typeof(T));
    }

    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate)
        where T : Attribute
    {
        return type.GetCustomAttributes<T>(inherit: true).Any(predicate);
    }

    public static bool IsBasedOn(this Type type, Type otherType) =>
        otherType.IsGenericTypeDefinition ?
            type.IsAssignableToGenericTypeDefinition(otherType)
            : otherType.IsAssignableFrom(type);

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

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) to type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type to compare to</typeparam>
    /// <param name="target">Type to check</param>
    /// <returns>True if <paramref name="target"/> is assignable to <typeparamref name="T"/></returns>
    public static bool AssignableTo<T>(this Type target) => target.AssignableTo(typeof(T));

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) to type <typeparamref name="T"/>
    /// </summary>
    /// <param name="target">Type to check</param>
    /// <param name="type">Type to compare to</param>
    /// <returns>True if <paramref name="target"/> is assignable to <typeparamref name="T"/></returns>
    public static bool AssignableTo(this Type target, Type type) => target.AssignableToAnyOf(type);

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) any of the types supplied in <paramref name="types"/>
    /// </summary>
    /// <param name="target">Type to check</param>
    /// <param name="types">Types to compare to</param>
    /// <returns>True if <paramref name="target"/> is assignable to any of <paramref name="types"/></returns>
    public static bool AssignableToAnyOf(this Type target, params Type[] types) => target.AssignableToAnyOf(types.AsEnumerable());

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) any of the types supplied in <paramref name="types"/>
    /// </summary>
    /// <param name="target">Type to check</param>
    /// <param name="types">Types to compare to</param>
    /// <returns>True if <paramref name="target"/> is assignable to any of <paramref name="types"/></returns>
    public static bool AssignableToAnyOf(this Type target, IEnumerable<Type> types) => types.Any(t => target.IsBasedOn(t));

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type to compare to</typeparam>
    /// <param name="target">Type to compare</param>
    /// <returns>True if <paramref name="target"/> is exactly of type <typeparamref name="T"/></returns>
    public static bool Exactly<T>(this Type? target) => target == typeof(T);

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of at least one of the types specified in <paramref name="types"/>
    /// </summary>
    /// <param name="target">Type to compare</param>
    /// <param name="types">Array of types to compare <paramref name="target"/> to</param>
    /// <returns>True if <paramref name="target"/> is exactly at least one of the types specified in <paramref name="types"/></returns>
    public static bool ExactlyAnyOf(this Type? target, params Type[] types) => target.ExactlyAnyOf(types.AsEnumerable());

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of at least one of the types specified in <paramref name="types"/>
    /// </summary>
    /// <param name="target">Type to compare</param>
    /// <param name="types">Array of types to compare <paramref name="target"/> to</param>
    /// <returns>True if <paramref name="target"/> is exactly at least one of the types specified in <paramref name="types"/></returns>
    public static bool ExactlyAnyOf(this Type? target, IEnumerable<Type> types) => types.Contains(target);

    private static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericType)
    {
        bool IsGenericTypeDefinitionMatch(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == genericType;

        return type.GetInterfaces().Any(IsGenericTypeDefinitionMatch) ||
               (type.IsGenericType && IsGenericTypeDefinitionMatch(type)) ||
               (type.BaseType != null && type.BaseType.IsAssignableToGenericTypeDefinition(genericType));
    }
}