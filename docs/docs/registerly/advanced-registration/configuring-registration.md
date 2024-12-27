# Configuring Class Registration
So far, we have been filtering the classes to register, but we can also configure the registration
of each class or group of classes individually.

Let's say we want to register all animals but with the following configuration:

- `Cat` should be registered as a `Transient` `IAnimal`.
- `Dog` should be registered as a `Singleton`, and as itself (i.e. as `Dog`).
- `Spider` and `Alligator` should be registered as `Scoped` with whichever interfaces they implement.

We can achieve this with the following configuration:

[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/Animals/Sample3.cs?highlight=15-21)]

> [!NOTE]
> We didn't specify any `Using` clause for `Alligator` and `Spider`, so they will 
be registered as using the default strategies `Scoped` and `AsImplementedInterfaces`.