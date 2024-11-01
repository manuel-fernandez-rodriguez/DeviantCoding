using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace DeviantCoding.Registerly.Scanning;

public delegate IQueryable<Type> SourceSelectorDelegate();
public delegate bool ClassFilterDelegate(Type type);

internal static class TypeScanner
{
    public static IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null)
        => FromDependencyContext(DependencyContext.Default ?? throw new InvalidOperationException("No default dependency context found"), _ => true, typeFilter ?? new ClassFilterDelegate(_ => true));

    public static IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .Where(assemblyFilter)
            .ToHashSet();

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public static IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
        => new AssemblyLoader().FromAssemblyNames(assemblyNames, typeFilter);

    public static IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
        => new AssemblyLoader().FromAssemblies(assemblies, typeFilter);

    public static IQueryable<Type> From(IEnumerable<Type> classes)
        => classes
            .Where(t => t.IsRegistrable())
            .AsQueryable();
}
