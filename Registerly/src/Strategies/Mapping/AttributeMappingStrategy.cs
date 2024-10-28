using DeviantCoding.Registerly.SelfRegistration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeviantCoding.Registerly.Strategies.Mapping;

internal class AttributeMappingStrategy : IMappingStrategy
{
    public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
    {
        return implementationType
                .GetCustomAttribute<RegisterlyAttribute>()?.
                MappingStrategy.Map(implementationType, lifetimeStrategy) ?? new AsImplementedInterfaces().Map(implementationType, lifetimeStrategy);
    }
}

