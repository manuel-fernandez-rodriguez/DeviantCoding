using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Scanning;


internal static class TypeSelector
{
    public static IEnumerable<Type> FromDependencyContext()
    {
        return FromDependencyContext(DependencyContext.Default, _ => true, _ => true);
    }

    public static IEnumerable<Type> FromDependencyContext(Func<Type, bool> typeFilter)
    {
        return FromDependencyContext(DependencyContext.Default, _ => true, typeFilter);
    }

    public static IEnumerable<Type> FromDependencyContext(DependencyContext context, Func<Assembly, bool> assemblyFilter, Func<Type, bool> typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .ToHashSet();

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public static IEnumerable<Type> FromAssemblyNames(IEnumerable<AssemblyName> assemblyNames, Func<Type, bool> typeFilter)
    {
        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public static IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, Func<Type, bool>? typeFilter = null)
    {
        return new AssemblyLoader()
            .FromAssemblies(assemblies, typeFilter);
    }
}
