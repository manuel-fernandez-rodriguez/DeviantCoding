# Introduction
Registerly is a nuget package that makes working with Microsoft Dependency Injection much easier.

Heavily inspired by [Scrutor](https://github.com/khellang/Scrutor), initially it had Scrutor as a dependency, but has later
evolved to become a full fledged replacement of Scrutor in most scenarios.

It's been designed from scratch to be easy to use and extensible.

# Usage

There are two main, non-exclusive, ways to use the package functionality, both require some instructions before building the `Host`: 

- Marking the classes we intend to register with some special attributes and invoking `builder.RegisterServicesByAttributes`.
- Selecting the classes to register by using `builder.Register` and, optionally applying a strategy to register them.

The former is the easiest way to register classes, while the latest is the most powerful and versatile.


