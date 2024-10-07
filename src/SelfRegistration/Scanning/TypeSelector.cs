using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.SelfRegistration.Scanning;


internal class AutoRegisterTypeSelector() : TypeSelector(t => t.IsNonAbstractClass(publicOnly: false) && t.HasAttribute<RegisterAsAttribute>())
{

}

internal class TypeSelector
{
    private readonly Func<Type, bool> _defaultTypeFilter;

    public TypeSelector(Func<Type, bool> defaultTypeFilter)
    {
        _defaultTypeFilter = defaultTypeFilter;
    }

    public IEnumerable<Type> FromDependencyContext()
    {
        return FromDependencyContext(DependencyContext.Default, _ => true, _ => true);
    }

    public IEnumerable<Type> FromDependencyContext(DependencyContext context, Func<Assembly, bool> assemblyFilter, Func<Type, bool> typeFilter)
    {
        var assemblyNames = context.RuntimeLibraries
            .SelectMany(library => library.GetDefaultAssemblyNames(context))
            .ToHashSet();

        return FromAssemblyNames(assemblyNames, assemblyFilter, typeFilter);
    }

    public IEnumerable<Type> FromAssemblyNames(IEnumerable<AssemblyName> assemblyNames, Func<Assembly, bool> assemblyFilter, Func<Type, bool> typeFilter)
    {
        var assemblies = LoadAssemblies(assemblyNames);
        return FromAssemblies(assemblies, typeFilter);
    }

    public IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, Func<Type, bool> typeFilter)
    {
        return assemblies
            .SelectMany(asm => asm.GetLoadableTypes())
            .Where(t => _defaultTypeFilter(t) && typeFilter(t));
    }

    private static ISet<Assembly> LoadAssemblies(IEnumerable<AssemblyName> assemblyNames)
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
