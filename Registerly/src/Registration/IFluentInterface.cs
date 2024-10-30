using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;


public interface IClassSource : IFluentInterface
{
    IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies);
    IClassSourceResult FromAssemblyOf<T>();
    IClassSourceResult FromClasses(IEnumerable<Type> candidates);
    IClassSourceResult FromDependencyContext();
    IClassSourceResult AndAlso(ClassFilterDelegate predicate);
}


public interface IClassSourceResult : IFluentInterface, IClassSource, ILifetimeDefinition, IMappingStrategyDefinition, IRegistrationStrategyDefinition, IRegisterServices
{
    IClassSourceResult Where(ClassFilterDelegate predicate);
}

public interface  IStrategyDefinitionResultResult : IClassSourceResult, IRegisterServices, IClassSource { }

public interface ILifetimeDefinition : IFluentInterface
{
    ILifetimeDefinitionResult WithLifetime(ILifetimeStrategy serviceLifetime);
}

public interface ILifetimeDefinitionResult : IFluentInterface, IStrategyDefinitionResultResult { }

public interface IMappingStrategyDefinition : IFluentInterface
{
    IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy);
}

public interface IMappingStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResultResult { }

public interface IRegistrationStrategyDefinition : IFluentInterface
{
    IRegistrationStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy);
}

public interface IRegistrationStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResultResult { }

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
