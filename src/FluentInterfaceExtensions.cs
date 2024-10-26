using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly
{
    public static class FluentInterfaceExtensions
    {

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

        public static UsingResult Using<TLifetime>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new() => target.Using<TLifetime, AsImplementedInterfaces>();

        public static ILifetimeDefinitionResult WithLifetime(this ILifetimeDefinition target, ServiceLifetime serviceLifetime)
            => target.WithLifetime(serviceLifetime.ToStrategy());
        public static UsingResult Using<TLifetime, TMappingStrategy>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new() => target.Using<TLifetime, TMappingStrategy, AddRegistrationStrategy>();

        public static UsingResult Using<TLifetime, TMappingStrategy, TRegistrationStrategy>(this IClassSourceResult target)
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new()
            where TRegistrationStrategy : IRegistrationStrategy, new()
        {
            return target.Using(new TLifetime(), new TMappingStrategy(), new TRegistrationStrategy());
        }

        public static UsingResult Using(this IClassSourceResult target, ServiceLifetime lifetime) => target.Using(lifetime.ToStrategy(), null!, null!);

        public static UsingResult Using(this IClassSourceResult target, ServiceLifetime lifetime, MappingStrategyEnum mappingStrategy) => target.Using(lifetime.ToStrategy(), MappingStrategy.From(mappingStrategy), null!);

        public static UsingResult Using(this IClassSourceResult target, ServiceLifetime lifetime, IMappingStrategy mappingStrategy) => target.Using(lifetime.ToStrategy(), mappingStrategy, null!);

        
        
        public static UsingResult Using(this IClassSourceResult target, RegisterlyAttribute attribute)
        {
            return target.Using(attribute.LifetimeStrategy, attribute.MappingStrategy, attribute.RegistrationStrategy);
        }
        
    }
}
