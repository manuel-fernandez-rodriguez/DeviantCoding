# Registering Open Generics
Microsoft dependency Injection supports open generics. This means that you can register a generic 
type and then resolve it with a specific type argument. 

This is useful when you have a generic service that you want to register and resolve with a specific 
type argument.

An example of this concept is the well-known generic `ILogger<T>` interface. 

```csharp
public class MyService(ILogger<MyService> logger)
{
    void MyMethod()
    {
        logger.LogInformation("The message");
    }
}
```

Registerly supports open generics registration, and we we'll see how to do it in the following sections.

## As Implemented Interfaces
Registering an open generic as implemented interfaces is done right off the bat. No need for any
additional considerations.

### Searching for the class to register 
[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/OpenGenerics/Sample.cs?highlight=18,21)]

The output of executing the preceding sample would be the following:
```
OpenGenericService`1
Int32
```

### Using Attributes
[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/OpenGenerics/Sample2.cs?highlight=17,20)]

The output of executing the preceding sample would be the following:
```
OpenGenericService`1
Int32
```

## As a specific interface
Registering a specific open generic implementation is also supported, but it requires some caveats to overcome
c# syntax limitations, both when using unbound generic types as type parameters, and also when using 
non-constant fields on attributes.

### Searching for the class to register
[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/OpenGenerics/Sample3.cs?highlight=21-22,25)]

The output of executing the preceding sample would be the following:
```
OpenGenericService`1
Int32
```

### Using Attributes
[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/OpenGenerics/Sample4.cs?highlight=14-16,22,25)]

The output of executing the preceding sample would be the following:
```
OpenGenericService`1
Int32
```