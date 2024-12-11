# Registration by Attributes

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
    public string Greet() => "Hello, World!"
}
```

Since it is a pure functional class, it makes sense to register it as a singleton, so that, whenever another class requests a
`IGreeterService`, the same instance of this class is returned.

## Preparing the application
First of all, we need to instruct our application to search for the attribute marked classes to be automatically registered.

We do that before building the host like this:

```csharp
builder.RegisterServicesByAttributes();

var app = builder.Build();
```

That's it. 

Now our application is ready to automatically register the classes we mark with some special attributes we will 
see with an example in the next section.

## Registering classes in DI

Fulfilling the requirement is as easy as marking the class with the `[Singleton]` attribute.

```csharp
using DeviantCoding.Registerly.AttributeRegistration;

[Singleton] // This will register GreeterService as Singleton with its implemented interfaces.
public class GreeterService : IGreeterService
{
    public string Greet() => "Hello, World!"
}
```

Besides the `[Singleton]` attribute, we can instead apply `[Scoped]` or `[Transient]` to register the class with the desired lifetime.
