# Custom Strategies

## Strategy Pattern
Under the hood, most of what we have been seeing in the preceding sections
is implemented using dierent `Strategies`.

There are 3 main kinds of strategies:

- `ILifetimeStrategy`: Responsible for setting the lifetime of the services.
- `IMappingStrategy`: Responsible for mapping the services to their implementation.
- `IRegistrationStrategy`: Responsible for registering the classes.

Note that they are all interfaces, so you can implement and use your own strategies.

A lifetime strategy is responsible for setting the lifetime of the services (`Singleton`, `Scoped`, `Transient`),

A mapping strategy translates a class to an IEnumerable<ServiceDescriptor>. 

For instance, the default mapping strategy, `AsImplementedInterfaces` returns this enumerable 
with one item for each implemented interface as service, and the class as its implementation.

Then, the returned enumerable is registered in DI by using an `IRegistrationStrategy`, if none is specified,
then the default registration strategy, called Add, is used.

## Implementing a Custom Strategy

Let’s develop this concept with an example of a Mapping Strategy that is already included in Registerly:
```csharp
public class As<T>() : As(typeof(T))
{
}

public class As(Type serviceType) : IMappingStrategy
{
    public IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy)
    {
        foreach (var implementationType in implementationTypes)
        {
            yield return new ServiceDescriptor(serviceType, implementationType, lifetimeStrategy.Map(implementationType));
        }
    }
}
```
This strategy maps the classes to a single service type, identified by `serviceType`.

We can use a custom strategy like this, when programatically registering classes:

[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Sample3.cs?highlight=18)]

Or like this, when using attributes:

```csharp
[Transient<As<IAnimal>>]
public class Cat : IAnimal
{
}
```
