using System.ComponentModel;
using System.Reflection;
using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly;

/// <summary>
/// Defines a source of classes for registration.
/// </summary>
public interface IClassSource : IFluentInterface
{
    /// <summary>
    /// Specifies assemblies to scan for classes.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>An instance of <see cref="IClassSourceResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.FromAssemblies(new[] { Assembly.GetExecutingAssembly() });
    /// </code>
    /// </example>
    IClassSourceResult FromAssemblies(IEnumerable<Assembly> assemblies);

    /// <summary>
    /// Specifies a collection of types to use as candidates for registration.
    /// </summary>
    /// <param name="candidates">The types to use as candidates.</param>
    /// <returns>An instance of <see cref="IClassSourceResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.From(new[] { typeof(MyClass) });
    /// </code>
    /// </example>
    IClassSourceResult From(IEnumerable<Type> candidates);

    /// <summary>
    /// Specifies that the dependency context should be used to find classes.
    /// </summary>
    /// <returns>An instance of <see cref="IClassSourceResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.FromDependencyContext();
    /// </code>
    /// </example>
    IClassSourceResult FromDependencyContext();

    /// <summary>
    /// Filters the classes based on a predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter classes.</param>
    /// <returns>An instance of <see cref="IClassSourceResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.Where(type => type.Name.StartsWith("My"));
    /// </code>
    /// </example>
    IClassSourceResult Where(ClassFilterDelegate predicate);
}

/// <summary>
/// Defines the result of a class source operation.
/// </summary>
public interface IClassSourceResult : IFluentInterface, IClassSource, ILifetimeDefinition, IMappingStrategyDefinition, IRegistrationStrategyDefinition, IRegistrationTaskSource
{
    /// <summary>
    /// Adds an additional filter to the class source result.
    /// </summary>
    /// <param name="predicate">The predicate to filter classes.</param>
    /// <returns>An instance of <see cref="IClassSourceResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.AndAlso(type => type.Namespace == "MyNamespace");
    /// </code>
    /// </example>
    IClassSourceResult AndAlso(ClassFilterDelegate predicate);

    /// <summary>
    /// Specifies the strategies to use for lifetime, mapping, and registration.
    /// </summary>
    /// <param name="lifetime">The lifetime strategy to use.</param>
    /// <param name="mappingStrategy">The mapping strategy to use.</param>
    /// <param name="registrationStrategy">The registration strategy to use.</param>
    /// <returns>An instance of <see cref="IStrategyDefinitionResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(new Singleton(), new MyMappingStrategy(), new MyRegistrationStrategy());
    /// </code>
    /// </example>
    IStrategyDefinitionResult Using(ILifetimeStrategy lifetime, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy);
}

/// <summary>
/// Defines the result of a strategy definition operation.
/// </summary>
public interface IStrategyDefinitionResult : IClassSourceResult, IRegistrationTaskSource, IClassSource
{
}

/// <summary>
/// Defines the lifetime definition for a class registration.
/// </summary>
public interface ILifetimeDefinition : IFluentInterface
{
    /// <summary>
    /// Specifies the lifetime strategy to use.
    /// </summary>
    /// <param name="serviceLifetime">The lifetime strategy to use.</param>
    /// <returns>An instance of <see cref="ILifetimeDefinitionResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = lifetimeDefinition.WithLifetime(new Singleton());
    /// </code>
    /// </example>
    ILifetimeDefinitionResult WithLifetime(ILifetimeStrategy serviceLifetime);
}

/// <summary>
/// Defines the result of a lifetime definition operation.
/// </summary>
public interface ILifetimeDefinitionResult : IFluentInterface, IStrategyDefinitionResult
{
}

/// <summary>
/// Defines the mapping strategy definition for a class registration.
/// </summary>
public interface IMappingStrategyDefinition : IFluentInterface
{
    /// <summary>
    /// Specifies the mapping strategy to use.
    /// </summary>
    /// <param name="mappingStrategy">The mapping strategy to use.</param>
    /// <returns>An instance of <see cref="IMappingStrategyDefinitionResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.WithMappingStrategy(new MyMappingStrategy());
    /// </code>
    /// </example>
    IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy mappingStrategy);
}

/// <summary>
/// Defines the result of a mapping strategy definition operation.
/// </summary>
public interface IMappingStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResult
{
}

/// <summary>
/// Defines the registration strategy definition for a class registration.
/// </summary>
public interface IRegistrationStrategyDefinition : IFluentInterface
{
    /// <summary>
    /// Specifies the registration strategy to use.
    /// </summary>
    /// <param name="registrationStrategy">The registration strategy to use.</param>
    /// <returns>An instance of <see cref="IRegistrationStrategyDefinitionResult"/>.</returns>
    /// <example>
    /// <code>
    /// var result = registrationStrategyDefinition.WithRegistrationStrategy(new MyRegistrationStrategy());
    /// </code>
    /// </example>
    IRegistrationStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy);
}

/// <summary>
/// Defines the result of a registration strategy definition operation.
/// </summary>
public interface IRegistrationStrategyDefinitionResult : IFluentInterface, IStrategyDefinitionResult
{
}

/// <summary>
/// Defines a source of registration tasks.
/// </summary>
public interface IRegistrationTaskSource : IFluentInterface, IEnumerable<IRegistrationTask>
{
}

/// <summary>
/// Provides a fluent interface for method chaining.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IFluentInterface
{
    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    Type GetType();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    string? ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object? obj);
}
