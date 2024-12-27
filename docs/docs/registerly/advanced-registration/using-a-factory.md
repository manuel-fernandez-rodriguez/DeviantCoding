# Using a Factory
Continuing with the chain of weird requirements, now we have received the following one:

> We need to register the classes in such a way that, each time an `IAnimal` is resolved,
a random animal is returned.

We can achieve this by using a factory method in the `Register` method:
[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/Animals/Sample4.cs?highlight=15-25)]

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