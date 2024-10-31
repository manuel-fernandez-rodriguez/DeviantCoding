using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using System.ComponentModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;


public interface IClassSource : IFluentInterface
{
    IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies);
    IClassSourceResult From(IEnumerable<Type> candidates);
    IClassSourceResult FromDependencyContext();
}

public interface IClassSourceResult : IFluentInterface, IClassSource, ILifetimeDefinition, IMappingStrategyDefinition, IRegistrationStrategyDefinition, IRegistrationTaskSource
{
    IClassSourceResult Where(ClassFilterDelegate predicate);
    IClassSourceResult AndAlso(ClassFilterDelegate predicate);
}

public interface  IStrategyDefinitionResult : IClassSourceResult, IRegistrationTaskSource, IClassSource { }

public interface ILifetimeDefinition : IFluentInterface
{
    ILifetimeDefinitionResult WithLifetime(ILifetimeStrategy serviceLifetime);
}

public interface ILifetimeDefinitionResult : IFluentInterface, IStrategyDefinitionResult { }

public interface IMappingStrategyDefinition : IFluentInterface
{
    IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy);
}

public interface IMappingStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResult { }

public interface IRegistrationStrategyDefinition : IFluentInterface
{
    IRegistrationStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy);
}

public interface IRegistrationStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResult { }

public interface IRegistrationTaskSource : IFluentInterface, IEnumerable<RegistrationTask> { }


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
