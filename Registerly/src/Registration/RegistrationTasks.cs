using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationTasks(RegistrationBuilder owner) : List<RegistrationTask>
{
    public RegistrationBuilder AddNew(SourceSelectorDelegate sourceSelector, ClassFilterDelegate? serviceSelector = null)
    {
        serviceSelector ??= _ => true;

        Add(new()
        {
            SourceSelector = sourceSelector,
            Classes = sourceSelector().Where(t => serviceSelector(t))
        });
        return owner;
    }
    public IClassSourceResult Where(ClassFilterDelegate predicate)
    {
        var task = this.LastOrDefault();
        if (task != null)
        {
            task.Classes = task.Classes.Where(t => predicate(t)).AsQueryable();
        }
        return owner;
    }

    public IClassSourceResult AndAlso(ClassFilterDelegate predicate)
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("There is no current class source. Invoke any of the From* methods before calling this one.");
        }
        return AddNew(this.Last().SourceSelector, predicate);
    }

    public ILifetimeDefinitionResult ApplyStrategy(ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var task in this)
        {
            task.LifetimeStrategy ??= lifetimeStrategy;
        }
        return owner;
    }
    public IMappingStrategyDefinitionResult ApplyStrategy(IMappingStrategy mappingStrategy)
    {
        foreach (var task in this)
        {
            task.MappingStrategy ??= mappingStrategy;
        }
        return owner;
    }
    public IRegistrationStrategyDefinitionResult ApplyStrategy(IRegistrationStrategy registrationStrategy)
    {
        foreach (var task in this)
        {
            task.RegistrationStrategy ??= registrationStrategy;
        }
        return owner;
    }

}
