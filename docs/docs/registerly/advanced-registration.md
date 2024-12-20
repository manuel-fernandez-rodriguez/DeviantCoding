# Advanced Registration

## Filtering classes to register
Besides the simple example that we saw in [class selection](getting-started/class-selection.md),
we can use way more complex and powerful expressions to search the classes to register.

For instance, let's consider the following classes:

[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Services.cs)]

It's a fairly common pattern to register several classes implementing the same common interface,
`IAnimal` in this case. 

We can use the `Registerly` library to register all classes implementing
`IAnimal` in a single line of code:

[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Sample.cs?highlight=13-15)]

> [!NOTE]
> `AssignableTo` is an extension method that receives a type and returns whether the type 
is assignable to the generic argument passed as parameter.

This would output:

```
Cat
Dog
Spider
Alligator
```

Now, what if we wanted to register all animals but the `Alligator`?

We can refine the clause in the `Where` method to filter the classes:

[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Sample2.cs?highlight=13-15)]

> [!NOTE]
> `Exactly` is an extension method that receives a type and returns wheter the type
is exactly the same as the generic argument passed as parameter.

This would output:

```
Cat
Dog
Spider
```

The `Where` method  receives a `Func<Type, bool>` predicate, so we can use any C# expression to filter 
the classes to register.

`Where`, and the provided extension methods on type, are some powerful tools which allow us to
fulfill most filtering requirements we might encounter.

## Configuring Class Registration
So far, we have been filtering the classes to register, but we can also configure the registration
of each class or group of classes individually.

Let's say we want to register all animals but with the following configuration:

- `Cat` should be registered as a `Transient` `IAnimal`.
- `Dog` should be registered as a `Singleton`, and as itself (i.e. as `Dog`).
- `Spider` and `Alligator` should be registered as `Scoped` with whichever interfaces they implement.

We can achieve this with the following configuration:

[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Sample3.cs?highlight=15-21)]

> [!NOTE]
> We didn't specify any `Using` clause for `Alligator` and `Spider`, so they will 
be registered as using the default strategies `Scoped` and `AsImplementedInterfaces`.

## Using a Factory
Continuing with the chain of weird requirements, now we have received the following one:

> We need to register the classes in such a way that, each time an `IAnimal` is resolved,
a random animal is returned.

We can achieve this by using a factory method in the `Register` method:
[!code-csharp[](samples/Registerly.Samples.AdvancedRegistration/Animals/Sample4.cs?highlight=15-25)]

The output will vary, since it's random, but it should resemble something like this:

```
Cat
Alligator
Dog
Dog
Alligator
```

Note that we're using two `With*` methods that allow us to configure our registration
exactly as required.