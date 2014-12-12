using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public static class ApplicationOnlyServiceDescriptorExtensions
    {
        public static ServiceDescriptor ApplicationOnlySingleton<TService, TImplementation>([NotNull]this ServiceDescriber describer)
            where TImplementation : TService
        {
            return new ApplicationOnlyServiceDescriptor(typeof(TService), typeof(TImplementation), LifecycleKind.Singleton);
        }

        public static ServiceDescriptor ApplicationOnlySingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type service, [NotNull] Type implementationType)
        {
            return new ApplicationOnlyServiceDescriptor(service, implementationType, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor ApplicationOnlySingleton<TService>([NotNull]this ServiceDescriber describer,
                                                                  [NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return new ApplicationOnlyServiceDescriptor(typeof(TService), implementationFactory, LifecycleKind.Singleton);
        }

        public static ServiceDescriptor ApplicationOnlySingleton([NotNull]this ServiceDescriber describer,
                                                        [NotNull] Type serviceType,
                                                        [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            return new ApplicationOnlyServiceDescriptor(serviceType, implementationFactory, LifecycleKind.Singleton);
        }
    }
}
