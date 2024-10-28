using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationBuilder : IClassSelector, IClassSourceResult, IMappingStrategyDefinitionResult, ILifetimeDefinitionResult, IUsingResult, IQueryable<Type>
{
    private readonly IServiceCollection _serviceCollection;

    public RegistrationBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        Tasks = new(this);
    }

    private RegistrationTasks Tasks { get; }
    
    public IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies) => Tasks.AddNew(() => TypeScanner.FromAssemblies(assemblies));

    public IClassSourceResult FromAssemblyOf<T>() => Tasks.AddNew(() => TypeScanner.FromAssemblies([typeof(T).Assembly]));

    public IClassSourceResult FromClasses(IEnumerable<Type> candidates) => Tasks.AddNew(() => TypeScanner.FromClasses(candidates));

    public IClassSourceResult FromDependencyContext() => Tasks.AddNew(() => TypeScanner.FromDependencyContext());

    IClassSourceResult IClassSourceResult.Where(ClassFilterDelegate predicate)
    {
        var task = Tasks.LastOrDefault();
        if (task != null)
        {
            task.Classes = task.Classes.Where(t => predicate(t)).AsQueryable();
        }
        return this;
    }

    IQueryable<Type> IClassSourceResult.Classes => this;

    IClassSourceResult IClassSelector.AndAlso(ClassFilterDelegate predicate)
    {
        if (Tasks.Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        
        return Tasks.AddNew(Tasks.Last().SourceSelector, predicate); ;
    }

    IUsingResult IClassSourceResult.Using(ILifetimeStrategy lifetimeStrategy, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
    {
        foreach (var task in Tasks)
        {
            task.LifetimeStrategy ??= lifetimeStrategy;
            task.MappingStrategy ??= mappingStrategy;
            task.RegistrationStrategy ??= registrationStrategy;
        }
        return this;
    }

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy serviceLifetime)
    {
        IClassSourceResult.Using(serviceLifetime, null!, null!);
        return this;
    }

    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy)
    {
        IClassSourceResult.Using(null!, mappingStrategy, null!);
        return this;
    }

    IMappingStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
    {
        IClassSourceResult.Using(null!, null!, registrationStrategy);
        return this;
    }

    public IServiceCollection RegisterServices()
    {
        foreach (var task in Tasks)
        {
            foreach (var candidate in task.Classes)
            {
                var serviceLifetime = task.LifetimeStrategy ?? new Scoped();
                var mappingStrategy = task.MappingStrategy ?? new AsImplementedInterfaces();
                var registrationStrategy = task.RegistrationStrategy ?? new AddRegistrationStrategy();

                var descriptors = mappingStrategy!.Map(candidate, serviceLifetime);
                registrationStrategy!.RegisterServices(_serviceCollection, descriptors);
            }
        }

        return _serviceCollection;
    }

    IClassSourceResult IClassSourceResult => this;

    Type IQueryable.ElementType { get; } = typeof(Type);

    Expression IQueryable.Expression => Tasks.GetClasses().Expression;

    IQueryProvider IQueryable.Provider => Tasks.GetClasses().Provider;

    IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => Tasks.GetClasses().GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => Tasks.GetClasses().GetEnumerator();
}
