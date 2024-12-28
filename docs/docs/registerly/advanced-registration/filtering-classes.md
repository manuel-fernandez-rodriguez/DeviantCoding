# Filtering classes to register
Besides the simple example that we saw in [class selection](~/docs/registerly/getting-started/class-selection.md),
we can use way more complex and powerful expressions to search for the classes to register.

For instance, let's consider the following classes:

[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/Animals/Services.cs)]

It's a fairly common pattern to register several classes implementing the same common interface,
`IAnimal` in this case. 

We can use the `Registerly` library to register all classes implementing
`IAnimal` in a single line of code:

[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/Animals/Sample.cs?highlight=13-15)]

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

[!code-csharp[](~/docs/registerly/samples/Registerly.Samples.AdvancedRegistration/Animals/Sample2.cs?highlight=13-15)]

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

`Where`, and the provided extension methods on `System.Type`, are some powerful tools which allow us to
fulfill most filtering requirements we might encounter.
