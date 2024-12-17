using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeviantCoding.Registerly;

/// <summary>
/// Contain helper methods to help selecting classes to register in DI
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines if the type is a non-abstract class.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="publicOnly">If true, only public types are considered.</param>
    /// <returns>True if the type is a non-abstract class; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).IsNonAbstractClass(true);
    /// </code>
    /// </example>
    public static bool IsNonAbstractClass(this Type type, bool publicOnly)
    {
        return !type.IsSpecialName &&
               type.IsClass &&
               !type.IsAbstract &&
               !type.HasAttribute<CompilerGeneratedAttribute>() &&
               (!publicOnly || type.IsPublic || type.IsNestedPublic);
    }

    /// <summary>
    /// Gets all base types and interfaces implemented by the type.
    /// </summary>
    /// <param name="type">The type to get base types for.</param>
    /// <returns>An enumerable of base types and interfaces.</returns>
    /// <example>
    /// <code>
    /// IEnumerable&lt;Type&gt; baseTypes = typeof(MyClass).GetBaseTypes();
    /// </code>
    /// </example>
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

    /// <summary>
    /// Determines if the type is in the specified namespace.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="namespace">The namespace to check against.</param>
    /// <returns>True if the type is in the specified namespace; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).IsInNamespace("MyNamespace");
    /// </code>
    /// </example>
    public static bool IsInNamespace(this Type type, string @namespace)
    {
        var typeNamespace = type.Namespace ?? string.Empty;
        return typeNamespace.StartsWith(@namespace, StringComparison.Ordinal) &&
               (typeNamespace.Length == @namespace.Length || typeNamespace[@namespace.Length] == '.');
    }

    /// <summary>
    /// Determines if the type is in the exact specified namespace.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="namespace">The namespace to check against.</param>
    /// <returns>True if the type is in the exact specified namespace; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).IsInExactNamespace("MyNamespace");
    /// </code>
    /// </example>
    public static bool IsInExactNamespace(this Type type, string @namespace)
    {
        return string.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines if the type has the specified attribute.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="attributeType">The attribute type to check for.</param>
    /// <returns>True if the type has the specified attribute; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).HasAttribute(typeof(MyAttribute));
    /// </code>
    /// </example>
    public static bool HasAttribute(this Type type, Type attributeType)
    {
        return type.IsDefined(attributeType, inherit: true);
    }

    /// <summary>
    /// Determines if the type has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The attribute type to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type has the specified attribute; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).HasAttribute&lt;MyAttribute&gt;();
    /// </code>
    /// </example>
    public static bool HasAttribute<T>(this Type type)
        where T : Attribute
    {
        return type.HasAttribute(typeof(T));
    }

    /// <summary>
    /// Determines if the type has the specified attribute that matches the predicate.
    /// </summary>
    /// <typeparam name="T">The attribute type to check for.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <param name="predicate">The predicate to match the attribute against.</param>
    /// <returns>True if the type has the specified attribute that matches the predicate; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).HasAttribute&lt;MyAttribute&gt;(attr => attr.SomeProperty == "value");
    /// </code>
    /// </example>
    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate)
        where T : Attribute
    {
        return type.GetCustomAttributes<T>(inherit: true).Any(predicate);
    }

    /// <summary>
    /// Determines if the type is based on the specified type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="otherType">The type to compare against.</param>
    /// <returns>True if the type is based on the specified type; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).IsBasedOn(typeof(MyBaseClass));
    /// </code>
    /// </example>
    public static bool IsBasedOn(this Type type, Type otherType) =>
        otherType.IsGenericTypeDefinition ?
            type.IsAssignableToGenericTypeDefinition(otherType)
            : otherType.IsAssignableFrom(type);

    /// <summary>
    /// Determines if the type is an open generic type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is an open generic type; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyGenericClass&lt;&gt;).IsOpenGeneric();
    /// </code>
    /// </example>
    public static bool IsOpenGeneric(this Type type)
    {
        return type.IsGenericTypeDefinition;
    }

    /// <summary>
    /// Determines if the generic arity of the interface type matches the type.
    /// </summary>
    /// <param name="interfaceType">The interface type to check.</param>
    /// <param name="type">The type to compare against.</param>
    /// <returns>True if the generic arity matches; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(IMyInterface&lt;T&gt;).HasMatchingGenericArity(typeof(MyClass&lt;T&gt;));
    /// </code>
    /// </example>
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

    /// <summary>
    /// Determines if the type has compatible generic arguments with the generic type definition.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="genericTypeDefinition">The generic type definition to compare against.</param>
    /// <returns>True if the generic arguments are compatible; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass&lt;T&gt;).HasCompatibleGenericArguments(typeof(IMyInterface&lt;&gt;));
    /// </code>
    /// </example>
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

    /// <summary>
    /// Gets the registration type for the interface type and type.
    /// </summary>
    /// <param name="interfaceType">The interface type to check.</param>
    /// <param name="type">The type to compare against.</param>
    /// <returns>The registration type.</returns>
    /// <example>
    /// <code>
    /// Type registrationType = typeof(IMyInterface).GetRegistrationType(typeof(MyClass));
    /// </code>
    /// </example>
    public static Type GetRegistrationType(this Type interfaceType, Type type)
    {
        if (type.IsGenericTypeDefinition && interfaceType.IsGenericType)
        {
            return interfaceType.GetGenericTypeDefinition();
        }

        return interfaceType;
    }

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to compare to.</typeparam>
    /// <param name="target">Type to check.</param>
    /// <returns>True if <paramref name="target"/> is assignable to <typeparamref name="T"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).AssignableTo&lt;IMyInterface&gt;();
    /// </code>
    /// </example>
    public static bool AssignableTo<T>(this Type target) => target.AssignableTo(typeof(T));

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) to type <paramref name="type"/>
    /// </summary>
    /// <param name="target">Type to check.</param>
    /// <param name="type">Type to compare to.</param>
    /// <returns>True if <paramref name="target"/> is assignable to <paramref name="type"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).AssignableTo(typeof(IMyInterface));
    /// </code>
    /// </example>
    public static bool AssignableTo(this Type target, Type type) => target.AssignableToAnyOf(type);

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) any of the types supplied in <paramref name="types"/>.
    /// </summary>
    /// <param name="target">Type to check.</param>
    /// <param name="types">Types to compare to.</param>
    /// <returns>True if <paramref name="target"/> is assignable to any of <paramref name="types"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).AssignableToAnyOf(typeof(IMyInterface1), typeof(IMyInterface2));
    /// </code>
    /// </example>
    public static bool AssignableToAnyOf(this Type target, params Type[] types) => target.AssignableToAnyOf(types.AsEnumerable());

    /// <summary>
    /// Determines whether <paramref name="target"/> is assignable (inherits or implements) any of the types supplied in <paramref name="types"/>.
    /// </summary>
    /// <param name="target">Type to check.</param>
    /// <param name="types">Types to compare to.</param>
    /// <returns>True if <paramref name="target"/> is assignable to any of <paramref name="types"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).AssignableToAnyOf(new[] { typeof(IMyInterface1), typeof(IMyInterface2) });
    /// </code>
    /// </example>
    public static bool AssignableToAnyOf(this Type target, IEnumerable<Type> types) => types.Any(t => target.IsBasedOn(t));

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type to compare to.</typeparam>
    /// <param name="target">Type to compare.</param>
    /// <returns>True if <paramref name="target"/> is exactly of type <typeparamref name="T"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).Exactly&lt;IMyInterface&gt;();
    /// </code>
    /// </example>
    public static bool Exactly<T>(this Type? target) => target == typeof(T);

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of at least one of the types specified in <paramref name="types"/>.
    /// </summary>
    /// <param name="target">Type to compare.</param>
    /// <param name="types">Array of types to compare <paramref name="target"/> to.</param>
    /// <returns>True if <paramref name="target"/> is exactly at least one of the types specified in <paramref name="types"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).ExactlyAnyOf(typeof(IMyInterface1), typeof(IMyInterface2));
    /// </code>
    /// </example>
    public static bool ExactlyAnyOf(this Type? target, params Type[] types) => target.ExactlyAnyOf(types.AsEnumerable());

    /// <summary>
    /// Determines whether the <paramref name="target"/> is exactly of at least one of the types specified in <paramref name="types"/>.
    /// </summary>
    /// <param name="target">Type to compare.</param>
    /// <param name="types">Array of types to compare <paramref name="target"/> to.</param>
    /// <returns>True if <paramref name="target"/> is exactly at least one of the types specified in <paramref name="types"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).ExactlyAnyOf(new[] { typeof(IMyInterface1), typeof(IMyInterface2) });
    /// </code>
    /// </example>
    public static bool ExactlyAnyOf(this Type? target, IEnumerable<Type> types) => types.Contains(target);

    /// <summary>
    /// Determines if the type is assignable to a generic type definition.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="genericType">The generic type definition to compare against.</param>
    /// <returns>True if the type is assignable to the generic type definition; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool result = typeof(MyClass).IsAssignableToGenericTypeDefinition(typeof(IMyGenericInterface&lt;&gt;));
    /// </code>
    /// </example>
    private static bool IsAssignableToGenericTypeDefinition(this Type type, Type genericType)
    {
        bool IsGenericTypeDefinitionMatch(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == genericType;

        return type.GetInterfaces().Any(IsGenericTypeDefinitionMatch) ||
               (type.IsGenericType && IsGenericTypeDefinitionMatch(type)) ||
               (type.BaseType != null && type.BaseType.IsAssignableToGenericTypeDefinition(genericType));
    }
}