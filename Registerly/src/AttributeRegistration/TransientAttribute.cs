using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;

namespace DeviantCoding.Registerly.AttributeRegistration;

/// <summary>
/// Specifies that a class should be registered as a transient in the dependency injection container.
/// </summary>
/// <example>
/// <code>
/// [Transient]
/// public class MyTransientService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute() : RegisterlyAttribute(new Transient(), null, null);

/// <summary>
/// Specifies that a class should be registered as a transient in the dependency injection container with a specified mapping strategy.
/// </summary>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <example>
/// <code>
/// [Transient&lt;MyMappingStrategy&gt;]
/// public class MyTransientService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy>() : RegisterlyAttribute(new Transient(), new TMappingStrategy(), null)
    where TMappingStrategy : IMappingStrategy, new();

/// <summary>
/// Specifies that a class should be registered as a transient in the dependency injection container with specified mapping and registration strategies.
/// </summary>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <typeparam name="TRegistrationStrategy">The registration strategy to use.</typeparam>
/// <example>
/// <code>
/// [Transient&lt;MyMappingStrategy, MyRegistrationStrategy&gt;]
/// public class MyTransientService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class TransientAttribute<TMappingStrategy, TRegistrationStrategy>() : RegisterlyAttribute(new Transient(), new TMappingStrategy(), new TRegistrationStrategy())
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();