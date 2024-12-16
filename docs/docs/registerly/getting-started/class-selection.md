# Registration by class selection

## Example requirement
Let's imagine we've got the following interface:

```csharp
public interface IGreeterService
{
    string Greet();
}
```

Implemented by this class:

```csharp
public class GreeterService : IGreeterService
{
    public string Greet() => "Hello, World!";
}
```

Since it is a pure functional class, it makes sense to register it as a singleton, so that, whenever another class requests a
`IGreeterService`, the same instance of this class is returned.

## Registering the service
We need to search for the aforementioned class and register it as a singleton with its implemented interfaces.

We would achieve our goal by adding the following instruction in the services registration phase:

```csharp
using DeviantCoding.Registerly;
[...]
builder.Register(classes => classes
    .Where(c => c.Exactly<WeatherService>())
    .Using<Singleton>());

var app = builder.Build();
[...]
```

> [!NOTE]
> If we intend to register the service as Scoped, we can omit the whole `As` clause, because `Scoped`
is the default LifetimeStrategy.

> [!NOTE]
> Using the registration in the example is not the most efficient way to register classes because
we're not specifying the assembly/assemblies we want to scan, so the library will scan all accesible
assemblies.

## Improving class scanning performance
For a more performant class scanning, one of the several `From*` clauses could be specified, like this:
```csharp
using DeviantCoding.Registerly;
[...]

builder.Register(classes => classes
        .FromAssemblyOf<GreeterService>() // Note this added line
        .Where(c => c.Exactly<GreeterService>()));

var app = builder.Build();
[...]

```
