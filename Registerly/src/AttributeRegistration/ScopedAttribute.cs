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