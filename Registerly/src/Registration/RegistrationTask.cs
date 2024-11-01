using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.Registration;

public class RegistrationTask
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

    internal (ILifetimeStrategy lifetime, IMappingStrategy mapping, IRegistrationStrategy registration) GetStrategies()
    {
        return (LifetimeStrategy ?? Default.LifetimeStrategy,
                MappingStrategy ?? Default.MappingStrategy,
                RegistrationStrategy ?? Default.RegistrationStrategy);
    }
}
