using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.AttributeRegistration;

/// <summary>
/// Marks a class for registration with a scoped lifetime.
/// </summary>
/// <example>
/// <code>
/// [Scoped]
/// public class MyScopedService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute() : RegisterlyAttribute(new Scoped(), null, null);

/// <summary>
/// Marks a class for registration with a scoped lifetime. Allows for a custom mapping strategy.
/// </summary>
/// <typeparam name="TMappingStrategy">The <see cref="IMappingStrategy"/> to use</typeparam>
/// <example>
/// <code>
/// [Scoped&lt;MyMappingStrategy&gt;]
/// public class MyScopedService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy>() : RegisterlyAttribute(new Scoped(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new();

/// <summary>
/// Specifies that a class should be registered as scoped in the dependency injection container with specified mapping and registration strategies.
/// </summary>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <typeparam name="TRegistrationStrategy">The registration strategy to use.</typeparam>
/// <example>
/// <code>
/// [Singleton&lt;MyMappingStrategy, MyRegistrationStrategy&gt;]
/// public class MyScopedService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class ScopedAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Scoped(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();