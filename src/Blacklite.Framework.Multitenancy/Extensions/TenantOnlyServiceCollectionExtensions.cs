using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class TenantOnlyServiceCollectionExtensions
    {
        public static IServiceCollection AddTenantOnlySingleton([NotNull] this IServiceCollection collection,
                                                            [NotNull] Type service,
                                                            [NotNull] Type implementationType)
        {
            var descriptor = new TenantOnlyServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddTenantOnlySingleton([NotNull] this IServiceCollection collection,
                                                      [NotNull] Type service,
                                                      [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            var descriptor = new TenantOnlyServiceDescriptor(service, implementationFactory, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddTenantOnlySingleton<TService, TImplementation>([NotNull] this IServiceCollection services)
        {
            return services.AddTenantOnlySingleton(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddTenantOnlySingleton([NotNull] this IServiceCollection services,
                                                            [NotNull] Type serviceType)
        {
            return services.AddTenantOnlySingleton(serviceType, serviceType);
        }

        public static IServiceCollection AddTenantOnlySingleton<TService>([NotNull] this IServiceCollection services)
        {
            return services.AddTenantOnlySingleton(typeof(TService));
        }

        public static IServiceCollection AddTenantOnlySingleton<TService>([NotNull] this IServiceCollection services,
                                                                      [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return services.AddTenantOnlySingleton(typeof(TService), implementationFactory);
        }
    }
}
