using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Scanning;
using DeviantCoding.Registerly.Strategies;
using DeviantCoding.Registerly.Strategies.Lifetime;
using DeviantCoding.Registerly.Strategies.Mapping;
using DeviantCoding.Registerly.Strategies.Registration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DeviantCoding.Registerly
{
    internal class RegistrationBuilder(IServiceCollection serviceCollection, Func<IEnumerable<Type>> sourceSelector)
        : IClassSourceResult, IMappingStrategyDefinitionResult, ILifetimeDefinitionResult, UsingResult
    {
        private class RegistrationTask
        {
            public required ClassFilterDelegate ServiceSelector { get; init; }
            public IMappingStrategy? MappingStrategy { get; set; }
            public IRegistrationStrategy? RegistrationStrategy { get; set; }
            public ServiceLifetime? ServiceLifetime { get; set; }
        }

        private List<RegistrationTask> Tasks { get; } = [];

        public IClassSourceResult AddClasses() => AddClasses(_ => true);
        public IClassSourceResult AddClasses(Func<Type, bool> serviceSelector)
        {
            Tasks.Add(new RegistrationTask { ServiceSelector = new ClassFilterDelegate(serviceSelector) });
            return this;
        }

        public UsingResult Using(ServiceLifetime lifetime) => Using(lifetime, null!, null!);

        public UsingResult Using(ServiceLifetime lifetime, MappingStrategyEnum mappingStrategy) => Using(lifetime, MappingStrategy.From(mappingStrategy), null!);

        public UsingResult Using(ServiceLifetime lifetime, IMappingStrategy mappingStrategy) => Using(lifetime, mappingStrategy, null!);

        public UsingResult Using<TLifetime>()
            where TLifetime : ILifetimeStrategy, new() => Using<TLifetime, AsImplementedInterfaces>();

        public UsingResult Using<TLifetime, TMappingStrategy>()
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new() => Using<TLifetime, TMappingStrategy, AddRegistrationStrategy>();

        public UsingResult Using<TLifetime, TMappingStrategy, TRegistrationStrategy>()
            where TLifetime : ILifetimeStrategy, new()
            where TMappingStrategy : IMappingStrategy, new()
            where TRegistrationStrategy : IRegistrationStrategy, new()
        {
            return Using(LifetimeStrategy.From(new TLifetime()), new TMappingStrategy(), new TRegistrationStrategy());
        }

        public UsingResult Using(ServiceLifetime lifetime, IMappingStrategy mappingStrategy, IRegistrationStrategy registrationStrategy)
        {
            WithLifetime(lifetime)
                .WithMappingStrategy(mappingStrategy)
                .WithRegistrationStrategy(registrationStrategy);
            return this;
        }

        public ILifetimeDefinitionResult WithLifetime(ServiceLifetime serviceLifetime)
        {
            EnsureTasks();
            foreach (var task in Tasks.Where(t => t.ServiceLifetime is null))
            {
                task.ServiceLifetime = serviceLifetime;
            }
            return this;
        }

        public IMappingStrategyDefinitionResult WithMappingStrategy<TStrategy>() where TStrategy : IMappingStrategy, new()
            => WithMappingStrategy(new TStrategy());

        public IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy  mappingStrategy)
        {
            EnsureTasks();
            foreach (var task in Tasks.Where(t => t.MappingStrategy is null))
            {
                task.MappingStrategy = mappingStrategy;
            }
            return this;
        }

        public IMappingStrategyDefinitionResult WithRegistrationStrategy<TStrategy>() where TStrategy : IRegistrationStrategy, new()
            => WithRegistrationStrategy(new TStrategy());

        public IMappingStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        {
            EnsureTasks();
            foreach (var task in Tasks.Where(t => t.RegistrationStrategy is null))
            {
                task.RegistrationStrategy = registrationStrategy;
            }
            return this;
        }

        public IServiceCollection Register()
        {
            EnsureTasks();
            var allCandidates = sourceSelector();
            foreach (var candidate in allCandidates)
            {
                foreach (var task in Tasks)
                {
                    if (task.ServiceSelector(candidate))
                    {
                        var serviceLifetime = task.ServiceLifetime ?? ServiceLifetime.Scoped;
                        var mappingStrategy = task.MappingStrategy ?? new AsImplementedInterfaces();
                        var registrationStrategy = task.RegistrationStrategy ?? new AddRegistrationStrategy();

                        var descriptors = mappingStrategy!.Map(candidate, serviceLifetime);
                        registrationStrategy!.RegisterServices(serviceCollection, descriptors);
                    }
                }
            }

            return serviceCollection;
        }

        private void EnsureTasks()
        {
            if (Tasks.Count == 0)
            {
                AddClasses();
            }
        }
    }
}
