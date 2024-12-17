using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.AttributeRegistration;

/// <summary>
/// Base attribute for registering classes with specified lifetime, mapping, and registration strategies.
/// </summary>
/// <typeparam name="TLifetimeStrategy">The lifetime strategy to use.</typeparam>
/// <typeparam name="TMappingStrategy">The mapping strategy to use.</typeparam>
/// <typeparam name="TRegistrationStrategy">The registration strategy to use.</typeparam>
/// <example>
/// <code>
/// [Registerly&lt;Singleton, MyMappingStrategy, MyRegistrationStrategy&gt;]
/// public class MyService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute<TLifetimeStrategy, TMappingStrategy, TRegistrationStrategy>()
    : RegisterlyAttribute(new TLifetimeStrategy(), new TMappingStrategy(), new TRegistrationStrategy())
    where TLifetimeStrategy : ILifetimeStrategy, new()
    where TMappingStrategy : IMappingStrategy, new()
    where TRegistrationStrategy : IRegistrationStrategy, new();

/// <summary>
/// Base attribute for registering classes with specified lifetime, mapping, and registration strategies.
/// </summary>
/// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
/// <param name="mappingStrategy">The mapping strategy to use.</param>
/// <param name="registrationStrategy">The registration strategy to use.</param>
/// <example>
/// <code>
/// [Registerly(new Singleton(), new MyMappingStrategy(), new MyRegistrationStrategy())]
/// public class MyService
/// {
///     // Implementation
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public class RegisterlyAttribute(ILifetimeStrategy? lifetimeStrategy, IMappingStrategy? mappingStrategy, IRegistrationStrategy? registrationStrategy) : Attribute
{
    /// <summary>
    /// Gets the lifetime strategy.
    /// </summary>
    public ILifetimeStrategy? LifetimeStrategy { get; } = lifetimeStrategy;

    /// <summary>
    /// Gets the mapping strategy.
    /// </summary>
    public IMappingStrategy? MappingStrategy { get; } = mappingStrategy;

    /// <summary>
    /// Gets the registration strategy.
    /// </summary>
    public IRegistrationStrategy? RegistrationStrategy { get; } = registrationStrategy;
}