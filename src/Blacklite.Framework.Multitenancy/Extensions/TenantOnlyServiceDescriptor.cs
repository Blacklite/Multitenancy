using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public class TenantOnlyServiceDescriptor : ServiceDescriptor
    {
        public TenantOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Func<IServiceProvider, object> factory, ServiceLifetime Lifetime)
            : base(serviceType, factory, Lifetime)
        {

        }

        public TenantOnlyServiceDescriptor([NotNull]Type serviceType, [NotNull]Type implementationType, ServiceLifetime Lifetime)
            : base(serviceType, implementationType, Lifetime)
        {

        }
        public new static TenantOnlyServiceDescriptor Singleton([NotNull] Type service, [NotNull] Type implementationType)
        {
            return new TenantOnlyServiceDescriptor(service, implementationType, ServiceLifetime.Singleton);
        }

        public new static TenantOnlyServiceDescriptor Singleton([NotNull] Type service, [NotNull] Func<IServiceProvider, object> implementationFactory)
        {
            return new TenantOnlyServiceDescriptor(service, implementationFactory, ServiceLifetime.Singleton);
        }

        public new static TenantOnlyServiceDescriptor Singleton<TService, TImplementation>()
        {
            return Singleton(typeof(TService), typeof(TImplementation));
        }

        public static TenantOnlyServiceDescriptor Singleton([NotNull] Type serviceType)
        {
            return Singleton(serviceType, serviceType);
        }

        public static TenantOnlyServiceDescriptor Singleton<TService>()
        {
            return Singleton(typeof(TService));
        }

        public new static TenantOnlyServiceDescriptor Singleton<TService>([NotNull] Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return Singleton(typeof(TService), implementationFactory);
        }
    }
}
