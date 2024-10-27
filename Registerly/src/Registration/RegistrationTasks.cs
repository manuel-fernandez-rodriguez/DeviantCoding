using DeviantCoding.Registerly.Scanning;

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

    public IQueryable<Type> GetClasses() => this.LastOrDefault()?.Classes ?? Enumerable.Empty<Type>().AsQueryable();
}
