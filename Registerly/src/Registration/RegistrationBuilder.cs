using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationBuilder(IServiceCollection serviceCollection) : List<RegistrationTask>,
    IClassSource, 
    IClassSourceResult, ILifetimeDefinitionResult, IMappingStrategyDefinitionResult, IRegistrationStrategyDefinitionResult, IStrategyDefinitionResultResult
{
    public IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies) => AddNew(() => TypeScanner.FromAssemblies(assemblies));

    public IClassSourceResult FromAssemblyOf<T>() => AddNew(() => TypeScanner.FromAssemblies([typeof(T).Assembly]));

    public IClassSourceResult FromClasses(IEnumerable<Type> candidates) => AddNew(() => TypeScanner.FromClasses(candidates));

    public IClassSourceResult FromDependencyContext() => AddNew(() => TypeScanner.FromDependencyContext());

    IClassSourceResult IClassSourceResult.Where(ClassFilterDelegate predicate)
    {
        var task = this.LastOrDefault();
        if (task != null)
        {
            task.Classes = task.Classes.Where(t => predicate(t)).AsQueryable();
        }
        return this;
    }

    IClassSourceResult IClassSourceResult.AndAlso(ClassFilterDelegate predicate)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        return AddNew(this.Last().SourceSelector, predicate);
    }

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy lifetimeStrategy)
    {
        ForEach(task => task.LifetimeStrategy ??= lifetimeStrategy);
        return this;
    }
    
    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy)
    {
        ForEach(task => task.MappingStrategy ??= mappingStrategy);
        return this;
    }

    IRegistrationStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
    {
        ForEach(task => task.RegistrationStrategy ??= registrationStrategy);
        return this;
    }
    
    public IServiceCollection RegisterServices()
    {
        foreach (var task in this)
        {
            foreach (var candidate in task.Classes)
            {
                var serviceLifetime = task.LifetimeStrategy ?? Default.LifetimeStrategy;
                var mappingStrategy = task.MappingStrategy ?? Default.MappingStrategy;
                var registrationStrategy = task.RegistrationStrategy ?? Default.RegistrationStrategy;

                var descriptors = mappingStrategy!.Map(candidate, serviceLifetime);
                registrationStrategy!.RegisterServices(serviceCollection, descriptors);
            }
        }

        return serviceCollection;
    }

    private RegistrationBuilder AddNew(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        serviceSelector ??= _ => true;

        Add(new()
        {
            SourceSelector = sourceSelector,
            Classes = sourceSelector().Where(t => serviceSelector(t))
        });
        return this;
    }
}
