using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly
{
    public static class FluentInterfaceExtensions
    {
        public static IMappingStrategyDefinitionResult AsImplementedInterfaces(this IMappingStrategyDefinition target)
            => target.WithMappingStrategy(new AsImplementedInterfaces());

        public static IMappingStrategyDefinitionResult AsSelf(this IMappingStrategyDefinition target)
            => target.WithMappingStrategy(new AsSelf());

        public static IMappingStrategyDefinitionResult As<T>(this IMappingStrategyDefinition target)
            => target.WithMappingStrategy(new As<T>());

        public static IMappingStrategyDefinitionResult As(this IMappingStrategyDefinition target, Type serviceType)
            => target.WithMappingStrategy(new As(serviceType));

        public static IMappingStrategyDefinitionResult WithFactory<TService>(this IMappingStrategyDefinition target, Func<IServiceProvider, TService> factory)
            where TService : notnull
            => target.WithMappingStrategy(new WithFactory<TService> (factory));

        public static IMappingStrategyDefinitionResult WithMappingStrategy<TStrategy>(this IMappingStrategyDefinition target )
            where TStrategy : IMappingStrategy, new()
            => target.WithMappingStrategy(new TStrategy());

        public static IMappingStrategyDefinitionResult WithRegistrationStrategy<TStrategy>(this IRegistrationStrategyDefinition target)
            where TStrategy : IRegistrationStrategy, new()
            => target.WithRegistrationStrategy(new TStrategy());

        public static ILifetimeStrategy ToStrategy(this ServiceLifetime serviceLifetime)
            => serviceLifetime switch
            {
                ServiceLifetime.Scoped => new Scoped(),
                ServiceLifetime.Singleton => new Singleton(),
                ServiceLifetime.Transient => new Transient(),
                _ => throw new NotImplementedException()
            };

        public static IUsingResult Using<TLifetime>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new()
            => target.Using<TLifetime, AsImplementedInterfaces>();

        public static ILifetimeDefinitionResult WithLifetime(this ILifetimeDefinition target, ServiceLifetime serviceLifetime)
            => target.WithLifetime(serviceLifetime.ToStrategy());

        public static IUsingResult Using<TLifetime, TMappingStrategy>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new()
            => target.Using<TLifetime, TMappingStrategy, AddRegistrationStrategy>();

        public static IUsingResult Using<TLifetime, TMappingStrategy, TRegistrationStrategy>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new()
            where TRegistrationStrategy : IRegistrationStrategy, new()
            => target.Using(new TLifetime(), new TMappingStrategy(), new TRegistrationStrategy());

        public static IUsingResult Using(this IClassSourceResult target, ServiceLifetime lifetime)
            => target.Using(lifetime.ToStrategy(), null!, null!);

        public static IUsingResult Using(this IClassSourceResult target, ServiceLifetime lifetime, IMappingStrategy mappingStrategy)
            => target.Using(lifetime.ToStrategy(), mappingStrategy, null!);
    }
}
