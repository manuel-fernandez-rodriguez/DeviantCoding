using System.Collections;
using System.Reflection;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationTaskBuilder() : IEnumerable<IRegistrationTask>,
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
            return FromDefaultSource(predicate);
        }

        GetCurrentTask().ApplyPredicate(predicate);
        return this;
    }

    IClassSourceResult IClassSourceResult.AndAlso(ClassFilterDelegate predicate)
        => AddNew(GetCurrentTask().SourceSelector, predicate);

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy lifetimeStrategy)
        => ForEach(task => task.Strategies.SetIfNull(lifetimeStrategy, null, null));

    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy)
        => ForEach(task => task.Strategies.SetIfNull(null, mappingStrategy, null));

    IRegistrationStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        => ForEach(task => task.Strategies.SetIfNull(null, null, registrationStrategy));

    IStrategyDefinitionResult IClassSourceResult.Using(ILifetimeStrategy lifetimeStrategy, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
        => ForEach(task => task.Strategies.SetIfNull(lifetimeStrategy, mappingStrategy, registrationStrategy));

    IEnumerator<IRegistrationTask> IEnumerable<IRegistrationTask>.GetEnumerator()
        => _tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _tasks.GetEnumerator();

    private RegistrationTaskBuilder AddNew(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        _tasks.Add(new(sourceSelector, serviceSelector));
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

    private RegistrationTask GetCurrentTask()
    {
        if (_tasks.Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }

        return _tasks.Last();
    }

    private IClassSourceResult FromDefaultSource(ClassFilterDelegate predicate)
        => ((IClassSource)this).FromDependencyContext().Where(predicate);
}
