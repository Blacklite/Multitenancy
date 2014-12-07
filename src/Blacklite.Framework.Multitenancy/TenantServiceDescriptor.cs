using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public class TenantServiceDescriptor : ServiceDescriptor
    {
        public TenantServiceDescriptor([NotNull]Type serviceType, [NotNull]Func<IServiceProvider, object> factory, LifecycleKind lifecycle)
            : base(serviceType, factory, lifecycle)
        {

        }

        public TenantServiceDescriptor([NotNull]Type serviceType, [NotNull]Type implementationType, LifecycleKind lifecycle)
            : base(serviceType, implementationType, lifecycle)
        {

        }
    }
}
