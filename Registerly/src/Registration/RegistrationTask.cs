using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Registration;

public interface IRegistrationTask
{
    IQueryable<Type> Classes { get; }
    Strategies Strategies { get; }
    SourceSelectorDelegate SourceSelector { get; }
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

public class Strategies
{
    public ILifetimeStrategy? LifetimeStrategy { get; private set; }

    public IMappingStrategy? MappingStrategy { get; private set; }

    public IRegistrationStrategy? RegistrationStrategy { get; private set; }

    internal FullySpecifiedStrategies ToFullySpecifiedStrategies() => new(this);

    internal void SetIfNull(ILifetimeStrategy? lifetimeStrategy, IMappingStrategy? mappingStrategy, IRegistrationStrategy? registrationStrategy)
    {
        LifetimeStrategy ??= lifetimeStrategy;
        MappingStrategy ??= mappingStrategy;
        RegistrationStrategy ??= registrationStrategy;
    }

}


internal class FullySpecifiedStrategies(Strategies strategies)
{
    public ILifetimeStrategy LifetimeStrategy { get; } = strategies.LifetimeStrategy ?? Default.LifetimeStrategy;

    public IMappingStrategy MappingStrategy { get; } = strategies.MappingStrategy ?? Default.MappingStrategy;

    public IRegistrationStrategy RegistrationStrategy { get; } = strategies.RegistrationStrategy ?? Default.RegistrationStrategy;

}
