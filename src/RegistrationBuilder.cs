using DeviantCoding.Registerly.Registration;
using DeviantCoding.Registerly.Strategies;
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
            public required Func<TypeInfo, bool> ServiceSelector { get; init; }
            public IMappingStrategy? MappingStrategy { get; set; }
            public IRegistrationStrategy? RegistrationStrategy { get; set; }
            public ServiceLifetime? ServiceLifetime { get; set; }
        }

        private List<RegistrationTask> _tasks = [];

        public IClassSourceResult AddClasses() => AddClasses(_ => true);
        public IClassSourceResult AddClasses(Func<TypeInfo, bool> serviceSelector)
        {
            _tasks.Add(new RegistrationTask { ServiceSelector = serviceSelector });
            return this;
        }

        public UsingResult Using(ServiceLifetime lifetime)
        {
            return Using(lifetime, null!, null!);
        }

        public UsingResult Using(ServiceLifetime lifetime, IMappingStrategy mappingStrategy)
        {
            return Using(lifetime, mappingStrategy, null!);
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
            foreach (var task in _tasks.Where(t => t.ServiceLifetime is null))
            {
                task.ServiceLifetime = serviceLifetime;
            }
            return this;
        }

        public IMappingStrategyDefinitionResult WithMappingStrategy<TStrategy>() where TStrategy : IMappingStrategy, new()
            => WithMappingStrategy(new TStrategy());

        public IMappingStrategyDefinitionResult WithMappingStrategy(IMappingStrategy  mappingStrategy)
        {
            foreach (var task in _tasks.Where(t => t.MappingStrategy is null))
            {
                task.MappingStrategy = mappingStrategy;
            }
            return this;
        }

        public IMappingStrategyDefinitionResult WithRegistrationStrategy<TStrategy>() where TStrategy : IRegistrationStrategy, new()
            => WithRegistrationStrategy(new TStrategy());

        public IMappingStrategyDefinitionResult WithRegistrationStrategy(IRegistrationStrategy registrationStrategy)
        {
            foreach (var task in _tasks.Where(t => t.RegistrationStrategy is null))
            {
                task.RegistrationStrategy = registrationStrategy;
            }
            return this;
        }


        public IServiceCollection Register()
        {
            var allCandidates = sourceSelector();
            foreach (var candidate in allCandidates)
            {
                foreach (var task in _tasks)
                {
                    if (task.ServiceSelector(candidate.GetTypeInfo()))
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

    }
}
