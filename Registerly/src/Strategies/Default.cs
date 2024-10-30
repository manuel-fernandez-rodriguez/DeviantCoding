namespace DeviantCoding.Registerly.Strategies;

internal static class Default
{
    public static ILifetimeStrategy LifetimeStrategy { get; } = new Lifetime.Scoped();
    public static IMappingStrategy MappingStrategy { get; } = new Mapping.AsImplementedInterfaces();
    public static IRegistrationStrategy RegistrationStrategy { get; } = new Registration.AddRegistrationStrategy();
}
