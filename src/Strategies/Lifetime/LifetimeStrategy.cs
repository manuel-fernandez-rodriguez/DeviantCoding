using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.Strategies.Lifetime
{

    public class Scoped : ILifetimeStrategy { }
    public class Singleton : ILifetimeStrategy { }
    public class Transient : ILifetimeStrategy { }

    public static class LifetimeStrategy
    {
        public static ServiceLifetime From<T>()
            where T : ILifetimeStrategy
        {
            var t = typeof(T);
            return true switch
            {
                var _ when t.IsAssignableTo(typeof(Scoped)) => ServiceLifetime.Scoped,
                var _ when t.IsAssignableTo(typeof(Singleton)) => ServiceLifetime.Singleton,
                var _ when t.IsAssignableTo(typeof(Transient)) => ServiceLifetime.Transient,
                _ => throw new NotImplementedException(),
            };
            ;
        }

        public static ServiceLifetime From(ILifetimeStrategy lifetimeStrategy)
        {
            return lifetimeStrategy switch
            {
                Scoped _ => ServiceLifetime.Scoped,
                Singleton _ => ServiceLifetime.Singleton,
                Transient _ => ServiceLifetime.Transient,
                _ => throw new NotImplementedException()
            };
        }
    }
}
