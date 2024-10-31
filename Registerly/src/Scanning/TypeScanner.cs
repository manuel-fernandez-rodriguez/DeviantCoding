using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Scanning;

public delegate IQueryable<Type> SourceSelectorDelegate();
public delegate bool ClassFilterDelegate(Type type);

internal static class TypeScanner
{
    public static IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null)
    {
        return FromDependencyContext(DependencyContext.Default, _ => true, typeFilter ?? new ClassFilterDelegate(_ => true));
    }

    public static IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .ToHashSet();

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames.Where(assemblyFilter), typeFilter)
            .AsQueryable();
    }

    public static IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
    {
        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter)
            .AsQueryable();
    }

    public static IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
    {
        return new AssemblyLoader()
            .FromAssemblies(assemblies, typeFilter)
            .AsQueryable();
    }

    public static IQueryable<Type> From(IEnumerable<Type> classes)
    {
        return classes
            .Where(t => t.IsRegistrable())
            .AsQueryable();
    }
}
