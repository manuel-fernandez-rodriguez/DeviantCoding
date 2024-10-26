using System.Reflection;

namespace DeviantCoding.Registerly.Scanning;

internal class AssemblyLoader
{
    private static readonly Func<Type, bool> DefaultFilter = t => t.IsRegistrable();

    private readonly Func<Type, bool> _defaultFilter = DefaultFilter;

    public AssemblyLoader(Func<Type, bool> defaultFilter)
    {
        _defaultFilter = defaultFilter;
    }

    public AssemblyLoader() : this(DefaultFilter)
    {

    }

    public IEnumerable<Type> FromAssemblyNames(IEnumerable<AssemblyName> assemblyNames, Func<Type, bool> typeFilter)
    {
        var assemblies = LoadAssemblies(assemblyNames);
        return FromAssemblies(assemblies, typeFilter);
    }


    public IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, Func<Type, bool>? typeFilter = null)
    {
        typeFilter ??= _defaultFilter;
        return assemblies
            .SelectMany(asm => asm.GetLoadableTypes())
            .Where(t => _defaultFilter(t) && typeFilter(t));
    }

    private static HashSet<Assembly> LoadAssemblies(IEnumerable<AssemblyName> assemblyNames)
    {
        var assemblies = new HashSet<Assembly>();

        foreach (var assemblyName in assemblyNames)
        {
            try
            {
                // Try to load the referenced assembly...
                assemblies.Add(Assembly.Load(assemblyName));
            }
            catch
            {
                // Failed to load assembly. Skip it.
            }
        }

        return assemblies;
    }
}
