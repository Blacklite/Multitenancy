using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [LifecyclePerTenant]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Singleton)]
    public class TenantDependencySingleton
    {
        public ITenant Tenant { get; }

        public int Number { get; set; }

        public TenantDependencySingleton(ITenant tenant)
        {
            Tenant = tenant;
        }
    }

    [LifecyclePerTenant]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Scoped)]
    public class TenantDependencyScoped
    {
        public ITenant Tenant { get; }

        public int Number { get; set; }

        public TenantDependencyScoped(ITenant tenant)
        {
            Tenant = tenant;
        }
    }

    [LifecyclePerTenant]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Transient)]
    public class TenantDependencyTransient
    {
        public ITenant Tenant { get; }

        public int Number { get; set; }

        public TenantDependencyTransient(ITenant tenant)
        {
            Tenant = tenant;
        }
    }
}