using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;

namespace DeviantCoding.Registerly.Registration;

public interface IClassSource : IFluentInterface
{
    IClassSourceResult AddClasses();
    IClassSourceResult AddClasses(Func<TypeInfo, bool> predicate);
}

public interface IClassSourceResult : IClassSource, ILifetimeDefinition, IRegisterServices
{
    UsingResult Using(ServiceLifetime lifetime);
    UsingResult Using(ServiceLifetime lifetime, IMappingStrategy mappingStrategy);
    UsingResult Using(ServiceLifetime lifetime, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy);
}

public interface  UsingResult : IClassSource, IRegisterServices
{
    
}

public interface ILifetimeDefinition : IClassSource
{
    ILifetimeDefinitionResult WithLifetime(ServiceLifetime serviceLifetime);
}

public interface ILifetimeDefinitionResult : IMappingStrategyDefinition
{
}

public interface IMappingStrategyDefinition : IFluentInterface
{
    IMappingStrategyDefinitionResult WithMappingStrategy<TStrategy>() where TStrategy : IMappingStrategy, new();
    IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy);
}

public interface IMappingStrategyDefinitionResult : IRegistrationStrategyDefinition, IRegisterServices, IClassSource
{
}

public interface IRegistrationStrategyDefinition : IFluentInterface, IRegisterServices, IClassSource
{
    IMappingStrategyDefinitionResult WithRegistrationStrategy<TStrategy>() where TStrategy : IRegistrationStrategy, new();
    IMappingStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy);
}


public interface IRegisterServices : IFluentInterface
{
    IServiceCollection Register();
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
