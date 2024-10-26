using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;


public interface IClassSelector : IFluentInterface
{
    IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies);
    IClassSourceResult FromAssemblyOf<T>();
    IClassSourceResult FromClasses(IEnumerable<Type> candidates);
    IClassSourceResult FromDependencyContext();
    IClassSourceResult AndAlso(ClassFilterDelegate predicate);
}


public interface IClassSourceResult : IFluentInterface, IRegisterServices, ILifetimeDefinition, IClassSelector
{
    IClassSourceQueryable Where(Func<Type, bool> predicate);
    UsingResult Using(ILifetimeStrategy lifetimeStrategy, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy);
}

public interface IClassSourceQueryable : IClassSourceResult
{
    IQueryable<Type> Types { get; }
}

public interface  UsingResult : IClassSourceResult, IRegisterServices, IClassSelector
{
    
}

public interface ILifetimeDefinition : IFluentInterface, IRegisterServices
{
    ILifetimeDefinitionResult WithLifetime(ILifetimeStrategy serviceLifetime);
}

public interface ILifetimeDefinitionResult : IMappingStrategyDefinition
{
}

public interface IMappingStrategyDefinition : IFluentInterface
{
    IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy);
}

public interface IMappingStrategyDefinitionResult : IRegistrationStrategyDefinition, IRegisterServices, IClassSourceResult, IClassSelector
{
}

public interface IRegistrationStrategyDefinition : IFluentInterface, IRegisterServices, IClassSourceResult
{
    IMappingStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy);
}


public interface IRegisterServices : IFluentInterface
{
    IServiceCollection RegisterServices();
}


[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    Type GetType();

    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();

    [EditorBrowsable(EditorBrowsableState.Never)]
    string? ToString();

    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object? obj);
}
