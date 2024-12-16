using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.AttributeRegistration;

/// <summary>
/// Specifies that a class should be registered as a singleton in the dependency injection container.
/// </summary>
/// <example>
/// <code>
/// [Singleton]
/// public class MySingletonService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute() : RegisterlyAttribute(new Singleton(), null, null);

/// <summary>
/// Specifies that a class should be registered as a singleton in the dependency injection container with a specified mapping strategy.
/// </summary>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <example>
/// <code>
/// [Singleton&lt;MyMappingStrategy&gt;]
/// public class MySingletonService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy>() : RegisterlyAttribute(new Singleton(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new();

/// <summary>
/// Specifies that a class should be registered as a singleton in the dependency injection container with specified mapping and registration strategies.
/// </summary>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <typeparam name="TRegistrationStrategy">The registration strategy to use.</typeparam>
/// <example>
/// <code>
/// [Singleton&lt;MyMappingStrategy, MyRegistrationStrategy&gt;]
/// public class MySingletonService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class SingletonAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Singleton(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();