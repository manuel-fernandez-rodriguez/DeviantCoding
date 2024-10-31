using System.Reflection;

namespace DeviantCoding.Registerly.Scanning;

internal class AssemblyLoader(ClassFilterDelegate defaultFilter)
{
    public static readonly ClassFilterDelegate DefaultFilter = t => t.IsRegistrable();

    public AssemblyLoader() : this(DefaultFilter)
    {

    }

    public IQueryable<Type> FromAssemblyNames(IEnumerable<AssemblyName> assemblyNames, ClassFilterDelegate typeFilter)
    {
        var assemblies = LoadAssemblies(assemblyNames);
        return FromAssemblies(assemblies, typeFilter);
    }


    public IQueryable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, ClassFilterDelegate? typeFilter = null)
    {
        typeFilter ??= defaultFilter;
        return assemblies
            .AsQueryable()
            .SelectMany(asm => asm.GetLoadableTypes())
            .Where(t => defaultFilter(t) && typeFilter(t));
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
