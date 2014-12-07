using Blacklite;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(MultitenancyServices.GetDefaultServices(configuration));
            return services;
        }

        public static IServiceCollection AddTenantSingleton([NotNull] this IServiceCollection collection,
                                                            [NotNull] Type service,
                                                            [NotNull] Type implementationType)
        {
            var descriptor = new ServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddTenantSingleton([NotNull] this IServiceCollection collection,
                                                      [NotNull] Type service,
                                                      [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            var descriptor = new ServiceDescriptor(service, implementationFactory, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddTenantSingleton<TService, TImplementation>([NotNull] this IServiceCollection services)
        {
            return services.AddTenantSingleton(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddTenantSingleton([NotNull] this IServiceCollection services,
                                                            [NotNull] Type serviceType)
        {
            return services.AddTenantSingleton(serviceType, serviceType);
        }

        public static IServiceCollection AddTenantSingleton<TService>([NotNull] this IServiceCollection services)
        {
            return services.AddTenantSingleton(typeof(TService));
        }

        public static IServiceCollection AddTenantSingleton<TService>([NotNull] this IServiceCollection services,
                                                                      [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return services.AddTenantSingleton(typeof(TService), implementationFactory);
        }
    }
}
