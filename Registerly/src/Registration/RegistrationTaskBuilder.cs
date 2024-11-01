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

    IClassSourceResult IClassSource.FromAssemblies(IEnumerable<Assembly> assemblies) 
        => AddNew(() => TypeScanner.From(assemblies));

    IClassSourceResult IClassSource.From(IEnumerable<Type> candidates) 
        => AddNew(() => TypeScanner.From(candidates));

    IClassSourceResult IClassSource.FromDependencyContext() 
        => AddNew(() => TypeScanner.FromDependencyContext());

    IClassSourceResult IClassSource.Where(ClassFilterDelegate predicate)
    {
        if (_tasks.Count == 0)
        {
            return ((IClassSource) this).FromDependencyContext().Where(predicate);
        }

        var task = GetLastElement();
        task.Classes = task.Classes.Where(t => predicate(t)).AsQueryable();
        return this;
    }

    IClassSourceResult IClassSourceResult.AndAlso(ClassFilterDelegate predicate) 
        => AddNew(GetLastElement().SourceSelector, predicate);

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy lifetimeStrategy)
        => ForEach(task => task.LifetimeStrategy ??= lifetimeStrategy);
    
    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy)
        => ForEach(task => task.MappingStrategy ??= mappingStrategy);

    IRegistrationStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        => ForEach(task => task.RegistrationStrategy ??= registrationStrategy);

    IEnumerator<RegistrationTask> IEnumerable<RegistrationTask>.GetEnumerator()
        => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _tasks.GetEnumerator();

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

    private RegistrationTask GetLastElement()
    {
        if (_tasks.Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        return _tasks.Last();
    }

    
}
