using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public class ApplicationOnlyServiceDescriptor : ServiceDescriptor
    {
        public ApplicationOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Func<IServiceProvider, object> factory, ServiceLifetime Lifetime)
            : base(serviceType, factory, Lifetime)
        {

        }

        public ApplicationOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Type implementationType, ServiceLifetime Lifetime)
            : base(serviceType, implementationType, Lifetime)
        {

        }
        public new static ApplicationOnlyServiceDescriptor Singleton([NotNull] Type service, [NotNull] Type implementationType)
        {
            return new ApplicationOnlyServiceDescriptor(service, implementationType, ServiceLifetime.Singleton);
        }

        public new static ApplicationOnlyServiceDescriptor Singleton([NotNull] Type service, [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            return new ApplicationOnlyServiceDescriptor(service, implementationFactory, ServiceLifetime.Singleton);
        }

        public new static ApplicationOnlyServiceDescriptor Singleton<TService, TImplementation>()
        {
            return Singleton(typeof(TService), typeof(TImplementation));
        }

        public static ApplicationOnlyServiceDescriptor Singleton([NotNull] Type serviceType)
        {
            return Singleton(serviceType, serviceType);
        }

        public static ApplicationOnlyServiceDescriptor Singleton<TService>()
        {
            return Singleton(typeof(TService));
        }

        public new static ApplicationOnlyServiceDescriptor Singleton<TService>([NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return Singleton(typeof(TService), implementationFactory);
        }
    }
}
