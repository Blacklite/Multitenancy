using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public static class TenantOnlyServiceDescriptorExtensions
    {
        public static ServiceDescriptor TenantOnlySingleton<TService, TImplementation>([NotNull]this ServiceDescriber describer)
            where TImplementation : TService
        {
            return new TenantOnlyServiceDescriptor(typeof(TService), typeof(TImplementation), LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantOnlySingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type service, [NotNull] Type implementationType)
        {
            return new TenantOnlyServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantOnlySingleton<TService>([NotNull]this ServiceDescriber describer,
                                                                  [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return new TenantOnlyServiceDescriptor(typeof(TService), implementationFactory, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantOnlySingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type serviceType,
                                                        [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            return new TenantOnlyServiceDescriptor(serviceType, implementationFactory, LifecycleKind.Singleton);
        }
    }
}
