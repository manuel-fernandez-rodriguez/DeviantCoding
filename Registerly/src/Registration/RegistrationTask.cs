using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Registration;

/// <summary>
/// Represents a task for registering services.
/// </summary>
public interface IRegistrationTask
{
    /// <summary>
    /// Gets the classes to be registered.
    /// </summary>
    IQueryable<Type> Classes { get; }

    /// <summary>
    /// Gets the strategies for registration.
    /// </summary>
    Strategies Strategies { get; }

    /// <summary>
    /// Gets the source selector delegate.
    /// </summary>
    SourceSelectorDelegate SourceSelector { get; }

    /// <summary>
    /// Registers the services in the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to register services in.</param>
    void RegisterIn(IServiceCollection services);
}

internal class RegistrationTask : IRegistrationTask
{
    public RegistrationTask(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        serviceSelector ??= _ => true;
        SourceSelector = sourceSelector;
        Classes = sourceSelector().Where(t => serviceSelector(t));
    }

    public SourceSelectorDelegate SourceSelector { get; init; }

    public Strategies Strategies { get; } = new Strategies();

    public IQueryable<Type> Classes { get; private set; } = Enumerable.Empty<Type>().AsQueryable();

    internal void ApplyPredicate(ClassFilterDelegate predicate)
    {
        Classes = Classes.Where(t => predicate(t)).AsQueryable();
    }

    internal void RegisterIn(IServiceCollection services)
    {
        var strategies = Strategies.ToFullySpecifiedStrategies();
        var descriptors = strategies.MappingStrategy.Map(Classes, strategies.LifetimeStrategy);
        strategies.RegistrationStrategy.RegisterServices(services, descriptors);
    }

    #region IRegistrationTask implementation
    IQueryable<Type> IRegistrationTask.Classes => Classes;
    Strategies IRegistrationTask.Strategies => Strategies;
    SourceSelectorDelegate IRegistrationTask.SourceSelector => SourceSelector;
    void IRegistrationTask.RegisterIn(IServiceCollection services) => RegisterIn(services);
    #endregion
}

/// <summary>
/// Represents the strategies for registration.
/// </summary>
public class Strategies
{
    /// <summary>
    /// Gets or sets the lifetime strategy.
    /// </summary>
    public ILifetimeStrategy? LifetimeStrategy { get; private set; }

    /// <summary>
    /// Gets or sets the mapping strategy.
    /// </summary>
    public IMappingStrategy? MappingStrategy { get; private set; }

    /// <summary>
    /// Gets or sets the registration strategy.
    /// </summary>
    public IRegistrationStrategy? RegistrationStrategy { get; private set; }

    /// <summary>
    /// Converts the strategies to fully specified strategies.
    /// </summary>
    /// <returns>An instance of <see cref="FullySpecifiedStrategies"/>.</returns>
    internal FullySpecifiedStrategies ToFullySpecifiedStrategies() => new(this);

    /// <summary>
    /// Sets the strategies if they are null.
    /// </summary>
    /// <param name="lifetimeStrategy">The lifetime strategy to set.</param>
    /// <param name="mappingStrategy">The mapping strategy to set.</param>
    /// <param name="registrationStrategy">The registration strategy to set.</param>
    internal void SetIfNull(ILifetimeStrategy? lifetimeStrategy, IMappingStrategy? mappingStrategy, IRegistrationStrategy? registrationStrategy)
    {
        LifetimeStrategy ??= lifetimeStrategy;
        MappingStrategy ??= mappingStrategy;
        RegistrationStrategy ??= registrationStrategy;
    }
}


/// <summary>
/// Represents fully specified strategies for registration.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FullySpecifiedStrategies"/> class.
/// </remarks>
/// <param name="strategies">The strategies to use.</param>
internal class FullySpecifiedStrategies(Strategies strategies)
{
    /// <summary>
    /// Gets the lifetime strategy.
    /// </summary>
    public ILifetimeStrategy LifetimeStrategy { get; } = strategies.LifetimeStrategy ?? Default.LifetimeStrategy;

    /// <summary>
    /// Gets the mapping strategy.
    /// </summary>
    public IMappingStrategy MappingStrategy { get; } = strategies.MappingStrategy ?? Default.MappingStrategy;

    /// <summary>
    /// Gets the registration strategy.
    /// </summary>
    public IRegistrationStrategy RegistrationStrategy { get; } = strategies.RegistrationStrategy ?? Default.RegistrationStrategy;
}
