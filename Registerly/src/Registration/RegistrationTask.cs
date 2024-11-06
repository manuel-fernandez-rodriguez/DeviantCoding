using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.Registration;

public interface IRegistrationTask
{
    IQueryable<Type> Classes { get; }
    ILifetimeStrategy? LifetimeStrategy { get; }
    IMappingStrategy? MappingStrategy { get; }
    IRegistrationStrategy? RegistrationStrategy { get; }
    SourceSelectorDelegate SourceSelector { get; }
    (ILifetimeStrategy lifetime, IMappingStrategy mapping, IRegistrationStrategy registration) GetStrategies();
}

internal class RegistrationTask : IRegistrationTask
{
    required public SourceSelectorDelegate SourceSelector { get; init; }

    public ILifetimeStrategy? LifetimeStrategy { get; internal set; }

    public IMappingStrategy? MappingStrategy { get; internal set; }

    public IRegistrationStrategy? RegistrationStrategy { get; internal set; }

    public IQueryable<Type> Classes { get; internal set; } = Enumerable.Empty<Type>().AsQueryable();

    internal void ApplyPredicate(ClassFilterDelegate predicate)
    {
        Classes = Classes.Where(t => predicate(t)).AsQueryable();
    }

    #region IRegistrationTask implementation
    IQueryable<Type> IRegistrationTask.Classes => Classes;
    ILifetimeStrategy? IRegistrationTask.LifetimeStrategy => LifetimeStrategy;
    IMappingStrategy? IRegistrationTask.MappingStrategy => MappingStrategy;
    IRegistrationStrategy? IRegistrationTask.RegistrationStrategy => RegistrationStrategy;
    SourceSelectorDelegate IRegistrationTask.SourceSelector => SourceSelector;
    (ILifetimeStrategy lifetime, IMappingStrategy mapping, IRegistrationStrategy registration) IRegistrationTask.GetStrategies()
    {
        return (LifetimeStrategy ?? Default.LifetimeStrategy,
                MappingStrategy ?? Default.MappingStrategy,
                RegistrationStrategy ?? Default.RegistrationStrategy);
    }


    #endregion
}
