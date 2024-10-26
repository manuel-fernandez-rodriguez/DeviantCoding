using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationBuilder(IServiceCollection serviceCollection)
    : IClassSelector, IClassSourceResult, IClassSourceQueryable, IMappingStrategyDefinitionResult, ILifetimeDefinitionResult, UsingResult, IQueryable<Type>
{
    private class RegistrationTask
    {
        public required SourceSelectorDelegate SourceSelector { get; init; }
        public IMappingStrategy? MappingStrategy { get; set; }
        public IRegistrationStrategy? RegistrationStrategy { get; set; }
        public ILifetimeStrategy? LifetimeStrategy { get; set; }
        public IQueryable<Type> Classes { get; set; } = Enumerable.Empty<Type>().AsQueryable();
    }

    private List<RegistrationTask> Tasks { get; } = [];
    
    public IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies)
    {
        return InitRegistrationTasks(() => TypeScanner.FromAssemblies(assemblies));
    }

    public IClassSourceResult FromAssemblyOf<T>()
    {
        return InitRegistrationTasks(() => TypeScanner.FromAssemblies([typeof(T).Assembly]));
    }

    public IClassSourceResult FromClasses(IEnumerable<Type> candidates)
    {
        return InitRegistrationTasks(() => TypeScanner.FromClasses(candidates));
    }

    public IClassSourceResult FromDependencyContext()
    {
        return InitRegistrationTasks(() => TypeScanner.FromDependencyContext());
    }

    public IClassSourceResult AndAlso(ClassFilterDelegate predicate)
    {
        if (Tasks.Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        return InitRegistrationTasks(Tasks.Last().SourceSelector, predicate);
    }

    public UsingResult Using(ILifetimeStrategy lifetimeStrategy, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
    {
        foreach (var task in Tasks)
        {
            task.LifetimeStrategy ??= lifetimeStrategy;
            task.MappingStrategy ??= mappingStrategy;
            task.RegistrationStrategy ??= registrationStrategy;
        }
        return this;
    }

    public ILifetimeDefinitionResult WithLifetime(ILifetimeStrategy serviceLifetime)
    {
        Using(serviceLifetime, null!, null!);
        return this;
    }

    public IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy)
    {
        Using(null!, mappingStrategy, null!);
        return this;
    }

    public IMappingStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
    {
        Using(null!, null!, registrationStrategy);
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
                registrationStrategy!.RegisterServices(serviceCollection, descriptors);
            }
        }

        return serviceCollection;
    }

    private IClassSourceResult InitRegistrationTasks(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        serviceSelector ??= _ => true;

        Tasks.Add(new()
        {
            SourceSelector = sourceSelector,
            Classes = sourceSelector().Where(t => serviceSelector(t))
        });

        return this;
    }

    Type IQueryable.ElementType { get; } = typeof(Type);
    Expression IQueryable.Expression => GetClasses().Expression ?? throw new InvalidOperationException("No classes have been selected yet.");
    IQueryProvider IQueryable.Provider => GetClasses().Provider ?? throw new InvalidOperationException("No classes have been selected yet.");

    IQueryable<Type> IClassSourceQueryable.Types => this;

    IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
    {
        return GetClasses().GetEnumerator() ?? throw new InvalidOperationException("No classes have been selected yet.");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetClasses().GetEnumerator() ?? throw new InvalidOperationException("No classes have been selected yet.");
    }

    private IQueryable<Type> GetClasses()
    {
        return Tasks.LastOrDefault()?.Classes ?? throw new InvalidOperationException("No classes have been selected yet.");
    }

    public IClassSourceQueryable Where(Func<Type, bool> predicate)
    {
        var task = Tasks.LastOrDefault() ?? throw new InvalidOperationException("No classes have been selected yet.");
        task.Classes = task.Classes.Where(predicate).AsQueryable();
        return this;
    }
}
