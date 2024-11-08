using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace DeviantCoding.Registerly.Scanning;

public delegate IQueryable<Type> SourceSelectorDelegate();
public delegate bool ClassFilterDelegate(Type type);

internal interface ITypeScanner
{
    IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null);
    IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter);
    IQueryable<Type> From(IEnumerable<Type> classes);
    IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null);
    IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter);
}

internal class TypeScanner : ITypeScanner
{
    public static TypeScanner Default { get; } = new();

    public IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null)
        => FromDependencyContext(DependencyContext.Default ?? throw new InvalidOperationException("No default dependency context found"), _ => true, typeFilter ?? new ClassFilterDelegate(_ => true));

    public IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .Where(assemblyFilter)
            .ToHashSet();

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    public IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
        => new AssemblyLoader().FromAssemblyNames(assemblyNames, typeFilter);

    public IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
        => new AssemblyLoader().FromAssemblies(assemblies, typeFilter);

    public IQueryable<Type> From(IEnumerable<Type> classes)
        => classes
            .Where(t => t.IsRegistrable())
            .AsQueryable();
}
