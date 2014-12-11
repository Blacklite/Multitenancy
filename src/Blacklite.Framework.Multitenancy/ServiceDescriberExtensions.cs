using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public static class ServiceDescriptorExtensions
    {
        public static ServiceDescriptor TenantSingleton<TService, TImplementation>([NotNull]this ServiceDescriber describer)
            where TImplementation : TService
        {
            return new TenantServiceDescriptor(typeof(TService), typeof(TImplementation), LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantSingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type service, [NotNull] Type implementationType)
        {
            return new TenantServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantSingleton<TService>([NotNull]this ServiceDescriber describer,
                                                                  [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return new TenantServiceDescriptor(typeof(TService), implementationFactory, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor TenantSingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type serviceType,
                                                        [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            return new TenantServiceDescriptor(serviceType, implementationFactory, LifecycleKind.Singleton);
        }
    }
}
