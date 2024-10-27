﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.Strategies.Mapping
{
    public class As<T>() : As(typeof(T))
    { }


    public class As(Type serviceType) : IMappingStrategy
    {
        public IEnumerable<ServiceDescriptor> Map(Type implementationType, ILifetimeStrategy lifetimeStrategy)
        {
            return [new ServiceDescriptor(serviceType, implementationType, lifetimeStrategy.Lifetime)];
        }
    }
}