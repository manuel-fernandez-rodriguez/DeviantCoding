using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationBuilder : IClassSource, 
    IClassSourceResult, ILifetimeDefinitionResult, IMappingStrategyDefinitionResult, IRegistrationStrategyDefinitionResult, IStrategyDefinitionResultResult
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

    IClassSourceResult IClassSourceResult.Where(ClassFilterDelegate predicate) => Tasks.Where(predicate);

    IClassSourceResult IClassSource.AndAlso(ClassFilterDelegate predicate) => Tasks.AndAlso(predicate);

    ILifetimeDefinitionResult ILifetimeDefinition.WithLifetime(ILifetimeStrategy serviceLifetime) => Tasks.ApplyStrategy(serviceLifetime);
    
    IMappingStrategyDefinitionResult IMappingStrategyDefinition.WithMappingStrategy(IMappingStrategy mappingStrategy) => Tasks.ApplyStrategy(mappingStrategy);

    IRegistrationStrategyDefinitionResult IRegistrationStrategyDefinition.WithRegistrationStrategy(IRegistrationStrategy registrationStrategy) => Tasks.ApplyStrategy(registrationStrategy);
    
    public IServiceCollection RegisterServices()
    {
        foreach (var task in Tasks)
        {
            foreach (var candidate in task.Classes)
            {
                var serviceLifetime = task.LifetimeStrategy ?? Default.LifetimeStrategy;
                var mappingStrategy = task.MappingStrategy ?? Default.MappingStrategy;
                var registrationStrategy = task.RegistrationStrategy ?? Default.RegistrationStrategy;

                var descriptors = mappingStrategy!.Map(candidate, serviceLifetime);
                registrationStrategy!.RegisterServices(_serviceCollection, descriptors);
            }
        }

        return _serviceCollection;
    }
}
