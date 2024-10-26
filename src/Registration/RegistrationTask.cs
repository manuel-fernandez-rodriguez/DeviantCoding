using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;

namespace DeviantCoding.Registerly.Registration;

internal class RegistrationTask
{
    public required SourceSelectorDelegate SourceSelector { get; init; }
    public IMappingStrategy? MappingStrategy { get; set; }
    public IRegistrationStrategy? RegistrationStrategy { get; set; }
    public ILifetimeStrategy? LifetimeStrategy { get; set; }
    public IQueryable<Type> Classes { get; set; } = Enumerable.Empty<Type>().AsQueryable();
}
