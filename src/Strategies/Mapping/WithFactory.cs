using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class WithFactory<TService>(Func<IServiceProvider, TService> factory) : IMappingStrategy
        where TService : notnull
    {
        
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
        {
            return [new ServiceDescriptor(typeof(TService), factory: s => factory(s), lifetimeStrategy.Lifetime)];
        }
    }

    public static class WithFactory
    {
        public static WithFactory<TService> Create<TService>(Func<IServiceProvider, TService> factory) where TService : notnull => new WithFactory<TService>(factory);
    }

    //public class WithFactory(Type serviceType, Func<IServiceProvider, object> factory) : IMappingStrategy
    //{
    //    public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
    //    {
    //        return [new ServiceDescriptor(serviceType, factory: factory, lifetimeStrategy.Lifetime)];
    //    }
    //}
}
