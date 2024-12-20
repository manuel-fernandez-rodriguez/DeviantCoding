using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace DeviantCoding.Registerly.Scanning;

/// <summary>
/// Delegate for selecting a source of types.
/// </summary>
/// <returns>An <see cref="IQueryable{Type}"/> representing the selected types.</returns>
/// <example>
/// <code>
/// SourceSelectorDelegate selector = () => new[] { typeof(MyClass) }.AsQueryable();
/// </code>
/// </example>
public delegate IQueryable<Type> SourceSelectorDelegate();
/// <summary>
/// Delegate for filtering types.
/// </summary>
/// <param name="type">The type to filter.</param>
/// <returns>True if the type matches the filter; otherwise, false.</returns>
/// <example>
/// <code>
/// ClassFilterDelegate filter = type => type.Name.StartsWith("My");
/// </code>
/// </example>
public delegate bool ClassFilterDelegate(Type type);

internal interface ITypeScanner
{
    IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null);
    IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter);
    IQueryable<Type> From(IEnumerable<Type> classes);
    IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null);
    IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter);
}

/// <summary>
/// Scans assemblies and types for registration.
/// </summary>
internal class TypeScanner : ITypeScanner
{
    /// <summary>
    /// Gets the default instance of the <see cref="TypeScanner"/> class.
    /// </summary>
    public static TypeScanner Default { get; } = new();

    /// <summary>
    /// Scans the default dependency context for types.
    /// </summary>
    /// <param name="typeFilter">The filter to apply to types.</param>
    /// <returns>An <see cref="IQueryable{Type}"/> representing the scanned types.</returns>
    /// <example>
    /// <code>
    /// var types = TypeScanner.Default.FromDependencyContext(type => type.Name.StartsWith("My"));
    /// </code>
    /// </example>
    public IQueryable<Type> FromDependencyContext(ClassFilterDelegate? typeFilter = null)
        => FromDependencyContext(DependencyContext.Default ?? throw new InvalidOperationException("No default dependency context found"), _ => true, typeFilter ?? new ClassFilterDelegate(_ => true));

    /// <summary>
    /// Scans the specified dependency context for types.
    /// </summary>
    /// <param name="context">The dependency context to scan.</param>
    /// <param name="assemblyFilter">The filter to apply to assemblies.</param>
    /// <param name="typeFilter">The filter to apply to types.</param>
    /// <returns>An <see cref="IQueryable{Type}"/> representing the scanned types.</returns>
    /// <example>
    /// <code>
    /// var types = TypeScanner.Default.FromDependencyContext(DependencyContext.Default, assembly => assembly.Name.StartsWith("My"), type => type.Name.StartsWith("My"));
    /// </code>
    /// </example>
    public IQueryable<Type> FromDependencyContext(DependencyContext context, Func<AssemblyName, bool> assemblyFilter, ClassFilterDelegate typeFilter)
    {
        var assemblyNames = new HashSet<AssemblyName>(context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .Where(assemblyFilter));

        return new AssemblyLoader()
            .FromAssemblyNames(assemblyNames, typeFilter);
    }

    /// <summary>
    /// Scans the specified assemblies for types.
    /// </summary>
    /// <param name="assemblyNames">The assembly names to scan.</param>
    /// <param name="typeFilter">The filter to apply to types.</param>
    /// <returns>An <see cref="IQueryable{Type}"/> representing the scanned types.</returns>
    /// <example>
    /// <code>
    /// var types = TypeScanner.Default.From(new[] { new AssemblyName("MyAssembly") }, type => type.Name.StartsWith("My"));
    /// </code>
    /// </example>
    public IQueryable<Type> From(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
        => new AssemblyLoader().FromAssemblyNames(assemblyNames, typeFilter);

    /// <summary>
    /// Scans the specified assemblies for types.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <param name="typeFilter">The filter to apply to types.</param>
    /// <returns>An <see cref="IQueryable{Type}"/> representing the scanned types.</returns>
    /// <example>
    /// <code>
    /// var types = TypeScanner.Default.From(new[] { Assembly.GetExecutingAssembly() }, type => type.Name.StartsWith("My"));
    /// </code>
    /// </example>
    public IQueryable<Type> From(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
        => new AssemblyLoader().FromAssemblies(assemblies, typeFilter);

    /// <summary>
    /// Scans the specified types.
    /// </summary>
    /// <param name="classes">The types to scan.</param>
    /// <returns>An <see cref="IQueryable{Type}"/> representing the scanned types.</returns>
    /// <example>
    /// <code>
    /// var types = TypeScanner.Default.From(new[] { typeof(MyClass) });
    /// </code>
    /// </example>
    public IQueryable<Type> From(IEnumerable<Type> classes)
        => classes
            .Where(t => t.IsRegistrable())
            .AsQueryable();
}
