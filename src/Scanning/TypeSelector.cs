﻿using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Scanning;

public delegate IEnumerable<Type> SourceSelectorDelegate();
public delegate bool ClassFilterDelegate(Type type);

internal static class TypeSelector
{
    public static IEnumerable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null)
    {
        return FromDependencyContext(DependencyContext.Default, _ => true, typeFilter ?? new ClassFilterDelegate(_ => true));
    }

    public static IEnumerable<Type> FromDependencyContext(DependencyContext context, Func<Assembly, bool> assemblyFilter, ClassFilterDelegate typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .ToHashSet();

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public static IEnumerable<Type> FromAssemblyNames(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
    {
        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public static IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
    {
        return new AssemblyLoader()
            .FromAssemblies(assemblies, typeFilter);
    }

    public static IEnumerable<Type> FromClasses(IEnumerable<Type> classes)
    {
        return classes.Where(t => t.IsRegistrable());
    }
}
