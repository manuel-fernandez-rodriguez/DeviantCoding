using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using System.Collections;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationTaskBuilder() : IEnumerable<RegistrationTask>,
    IClassSource, 
    IClassSourceResult, ILifetimeDefinitionResult, IMappingStrategyDefinitionResult, IRegistrationStrategyDefinitionResult, IStrategyDefinitionResult
{

    private readonly List<RegistrationTask> _tasks = [];

    public IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies) => AddNew(() => TypeScanner.From(assemblies));

    public IClassSourceResult From(IEnumerable<Type> candidates) => AddNew(() => TypeScanner.From(candidates));

    public IClassSourceResult FromDependencyContext() => AddNew(() => TypeScanner.FromDependencyContext());

    IClassSourceResult IClassSourceResult.Where(ClassFilterDelegate predicate)
    {
        var task = _tasks.LastOrDefault();
        if (task != null)
        {
            task.Classes = task.Classes.Where(t => predicate(t)).AsQueryable();
        }
        return this;
    }

    IClassSourceResult IClassSourceResult.AndAlso(ClassFilterDelegate predicate)
    {
        if (_tasks.Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        return AddNew(_tasks.Last().SourceSelector, predicate);
    }

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy lifetimeStrategy)
        => ForEach(task => task.LifetimeStrategy ??= lifetimeStrategy);
    
    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy)
        => ForEach(task => task.MappingStrategy ??= mappingStrategy);

    IRegistrationStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        => ForEach(task => task.RegistrationStrategy ??= registrationStrategy);

    public IEnumerator<RegistrationTask> GetEnumerator() => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _tasks.GetEnumerator();

    private RegistrationTaskBuilder AddNew(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        serviceSelector ??= _ => true;

        _tasks.Add(new()
        {
            SourceSelector = sourceSelector,
            Classes = sourceSelector().Where(t => serviceSelector(t))
        });
        return this;
    }

    private RegistrationTaskBuilder ForEach(Action<RegistrationTask> action)
    {
        foreach (var task in _tasks)
        {
            action(task);
        }
        return this;
    }
}
