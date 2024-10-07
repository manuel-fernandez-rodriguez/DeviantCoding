﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DeviantCoding.Registerly.SelfRegistration.Mapping
{
    public class AsImplementedInterfaces : IRegistrationStrategy<AsImplementedInterfaces>
    {
        public static AsImplementedInterfaces Instance { get; } = new AsImplementedInterfaces();

        public IServiceCollection RegisterServices(IServiceCollection serviceCollection, Type implementation, ServiceLifetime serviceLifetime)
        {
            var services = implementation.GetInterfaces();
            var descriptors = services.Select(service => new ServiceDescriptor(service, implementation, serviceLifetime));

            foreach (var descriptor in descriptors)
            {
                serviceCollection.Add(descriptor);
            }
            return serviceCollection;
        }
    }
}
