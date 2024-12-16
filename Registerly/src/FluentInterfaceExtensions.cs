using System.Reflection;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly;

/// <summary>
/// Provides extension methods for fluent interface usage.
/// </summary>
public static class FluentInterfaceExtensions
{
    /// <summary>
    /// Specifies the assembly to scan for classes.
    /// </summary>
    /// <param name="source">The class source.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>The class source result.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.FromAssembly(Assembly.GetExecutingAssembly());
    /// </code>
    /// </example>
    public static IClassSourceResult FromAssembly(this IClassSource source, Assembly assembly)
        => source.FromAssemblies([assembly]);

    /// <summary>
    /// Specifies the assembly of the given type to scan for classes.
    /// </summary>
    /// <typeparam name="T">The type whose assembly to scan.</typeparam>
    /// <param name="source">The class source.</param>
    /// <returns>The class source result.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.FromAssemblyOf&lt;SomeType&gt;();
    /// </code>
    /// </example>
    public static IClassSourceResult FromAssemblyOf<T>(this IClassSource source)
        => source.FromAssembly(typeof(T).Assembly);

    /// <summary>
    /// Maps the classes to their implemented interfaces.
    /// </summary>
    /// <param name="target">The mapping strategy definition.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.AsImplementedInterfaces();
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult AsImplementedInterfaces(this IMappingStrategyDefinition target)
        => target.WithMappingStrategy(new AsImplementedInterfaces());

    /// <summary>
    /// Maps the classes to themselves.
    /// </summary>
    /// <param name="target">The mapping strategy definition.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.AsSelf();
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult AsSelf(this IMappingStrategyDefinition target)
        => target.WithMappingStrategy(new AsSelf());

    /// <summary>
    /// Maps the classes to the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="target">The mapping strategy definition.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.As&lt;IMyService&gt;();
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult As<T>(this IMappingStrategyDefinition target)
        => target.WithMappingStrategy(new As<T>());

    /// <summary>
    /// Maps the classes to the specified service type.
    /// </summary>
    /// <param name="target">The mapping strategy definition.</param>
    /// <param name="serviceType">The service type.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.As(typeof(IMyService));
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult As(this IMappingStrategyDefinition target, Type serviceType)
        => target.WithMappingStrategy(new As(serviceType));

    /// <summary>
    /// Uses a factory method to create the service.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <param name="target">The mapping strategy definition.</param>
    /// <param name="factory">The factory method.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.WithFactory&lt;IMyService&gt;(provider => new MyService());
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult WithFactory<TService>(this IMappingStrategyDefinition target, Func<IServiceProvider, TService> factory)
        where TService : notnull
        => target.WithMappingStrategy(new WithFactory<TService>(factory));

    /// <summary>
    /// Uses the specified mapping strategy.
    /// </summary>
    /// <typeparam name="TStrategy">The mapping strategy type.</typeparam>
    /// <param name="target">The mapping strategy definition.</param>
    /// <returns>The mapping strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = mappingStrategyDefinition.WithMappingStrategy&lt;CustomMappingStrategy&gt;();
    /// </code>
    /// </example>
    public static IMappingStrategyDefinitionResult WithMappingStrategy<TStrategy>(this IMappingStrategyDefinition target)
        where TStrategy : IMappingStrategy, new()
        => target.WithMappingStrategy(new TStrategy());

    /// <summary>
    /// Uses the specified registration strategy.
    /// </summary>
    /// <typeparam name="TStrategy">The registration strategy type.</typeparam>
    /// <param name="target">The registration strategy definition.</param>
    /// <returns>The registration strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = registrationStrategyDefinition.WithRegistrationStrategy&lt;CustomRegistrationStrategy&gt;();
    /// </code>
    /// </example>
    public static IRegistrationStrategyDefinitionResult WithRegistrationStrategy<TStrategy>(this IRegistrationStrategyDefinition target)
        where TStrategy : IRegistrationStrategy, new()
        => target.WithRegistrationStrategy(new TStrategy());

    /// <summary>
    /// Converts the service lifetime to a lifetime strategy.
    /// </summary>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>The lifetime strategy.</returns>
    /// <example>
    /// <code>
    /// var strategy = ServiceLifetime.Singleton.ToStrategy();
    /// </code>
    /// </example>
    public static ILifetimeStrategy ToStrategy(this ServiceLifetime serviceLifetime)
        => serviceLifetime switch
        {
            ServiceLifetime.Scoped => new Scoped(),
            ServiceLifetime.Singleton => new Singleton(),
            ServiceLifetime.Transient => new Transient(),
            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Specifies the lifetime strategy for the services.
    /// </summary>
    /// <param name="target">The lifetime definition.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>The lifetime definition result.</returns>
    /// <example>
    /// <code>
    /// var result = lifetimeDefinition.WithLifetime(ServiceLifetime.Singleton);
    /// </code>
    /// </example>
    public static ILifetimeDefinitionResult WithLifetime(this ILifetimeDefinition target, ServiceLifetime serviceLifetime)
        => target.WithLifetime(serviceLifetime.ToStrategy());

    /// <summary>
    /// Uses the specified lifetime strategy.
    /// </summary>
    /// <typeparam name="TLifetime">The lifetime strategy type.</typeparam>
    /// <param name="target">The class source result.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using&lt;Singleton&gt;();
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using<TLifetime>(this IClassSourceResult target)
        where TLifetime : ILifetimeStrategy, new()
        => target.Using<TLifetime, AsImplementedInterfaces>();

    /// <summary>
    /// Uses the specified lifetime and mapping strategies.
    /// </summary>
    /// <typeparam name="TLifetime">The lifetime strategy type.</typeparam>
    /// <typeparam name="TMappingStrategy">The mapping strategy type.</typeparam>
    /// <param name="target">The class source result.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using&lt;Singleton, AsSelf&gt;();
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using<TLifetime, TMappingStrategy>(this IClassSourceResult target)
        where TLifetime : ILifetimeStrategy, new()
        where TMappingStrategy : IMappingStrategy, new()
        => target.Using<TLifetime, TMappingStrategy, Add>();

    /// <summary>
    /// Uses the specified lifetime, mapping, and registration strategies.
    /// </summary>
    /// <typeparam name="TLifetime">The lifetime strategy type.</typeparam>
    /// <typeparam name="TMappingStrategy">The mapping strategy type.</typeparam>
    /// <typeparam name="TRegistrationStrategy">The registration strategy type.</typeparam>
    /// <param name="target">The class source result.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using&lt;Singleton, AsSelf, Add&gt;();
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using<TLifetime, TMappingStrategy, TRegistrationStrategy>(this IClassSourceResult target)
        where TLifetime : ILifetimeStrategy, new()
        where TMappingStrategy : IMappingStrategy, new()
        where TRegistrationStrategy : IRegistrationStrategy, new()
        => target.Using(new TLifetime(), new TMappingStrategy(), new TRegistrationStrategy());

    /// <summary>
    /// Uses the specified service lifetime.
    /// </summary>
    /// <param name="target">The class source result.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(ServiceLifetime.Singleton);
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using(this IClassSourceResult target, ServiceLifetime lifetime)
        => target.Using(lifetime.ToStrategy());

    /// <summary>
    /// Uses the specified service lifetime and mapping strategy.
    /// </summary>
    /// <param name="target">The class source result.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <param name="mappingStrategy">The mapping strategy.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(ServiceLifetime.Singleton, new CustomMappingStrategy());
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using(this IClassSourceResult target, ServiceLifetime lifetime, IMappingStrategy mappingStrategy)
        => target
            .Using(lifetime.ToStrategy(), mappingStrategy);

    /// <summary>
    /// Uses the specified service lifetime, mapping strategy, and registration strategy.
    /// </summary>
    /// <param name="target">The class source result.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <param name="mappingStrategy">The mapping strategy.</param>
    /// <param name="registrationStrategy">The registration strategy.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(ServiceLifetime.Singleton, new CustomMappingStrategy(), new CustomRegistrationStrategy());
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using(this IClassSourceResult target, ServiceLifetime lifetime, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
        => target
            .Using(lifetime.ToStrategy(), mappingStrategy, registrationStrategy);

    /// <summary>
    /// Uses the specified lifetime strategy.
    /// </summary>
    /// <param name="target">The class source result.</param>
    /// <param name="lifetime">The lifetime strategy.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(new Singleton());
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using(this IClassSourceResult target, ILifetimeStrategy lifetime)
        => target.Using(lifetime, Default.MappingStrategy, Default.RegistrationStrategy);

    /// <summary>
    /// Uses the specified lifetime and mapping strategies.
    /// </summary>
    /// <param name="target">The class source result.</param>
    /// <param name="lifetime">The lifetime strategy.</param>
    /// <param name="mappingStrategy">The mapping strategy.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSourceResult.Using(new Singleton(), new CustomMappingStrategy());
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult Using(this IClassSourceResult target, ILifetimeStrategy lifetime, IMappingStrategy mappingStrategy)
        => target.Using(lifetime, mappingStrategy, Default.RegistrationStrategy);

    /// <summary>
    /// Uses attributes to determine the strategies for the classes.
    /// </summary>
    /// <param name="target">The class source.</param>
    /// <returns>The strategy definition result.</returns>
    /// <example>
    /// <code>
    /// var result = classSource.UsingAttributes();
    /// </code>
    /// </example>
    public static IStrategyDefinitionResult UsingAttributes(this IClassSource target)
        => target.Where(t => t.IsMarkedForAutoRegistration())
            .Using<AttributeLifetimeStrategy, AttributeMappingStrategy, AttributeRegistrationStrategy>();
}