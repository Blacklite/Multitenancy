using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ApplicationOnlyServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationOnlySingleton([NotNull] this IServiceCollection collection,
                                                            [NotNull] Type service,
                                                            [NotNull] Type implementationType)
        {
            var descriptor = new TenantOnlyServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddApplicationOnlySingleton([NotNull] this IServiceCollection collection,
                                                      [NotNull] Type service,
                                                      [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            var descriptor = new TenantOnlyServiceDescriptor(service, implementationFactory, LifecycleKind.Singleton);
            return collection.Add(descriptor);
        }

        public static IServiceCollection AddApplicationOnlySingleton<TService, TImplementation>([NotNull] this IServiceCollection services)
        {
            return services.AddApplicationOnlySingleton(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddApplicationOnlySingleton([NotNull] this IServiceCollection services,
                                                            [NotNull] Type serviceType)
        {
            return services.AddApplicationOnlySingleton(serviceType, serviceType);
        }

        public static IServiceCollection AddApplicationOnlySingleton<TService>([NotNull] this IServiceCollection services)
        {
            return services.AddApplicationOnlySingleton(typeof(TService));
        }

        public static IServiceCollection AddApplicationOnlySingleton<TService>([NotNull] this IServiceCollection services,
                                                                      [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return services.AddApplicationOnlySingleton(typeof(TService), implementationFactory);
        }
    }
}
