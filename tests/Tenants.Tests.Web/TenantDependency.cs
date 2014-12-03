using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [LifecyclePerTenant]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Singleton)]
    public class TenantDependency
    {
        public ITenant Tenant { get; }

        public int Number { get; }

        public TenantDependency(ITenant tenant)
        {
            Tenant = tenant;
        }
    }

    [LifecyclePerTenant]
    [ServiceDescriptor(Lifecycle = LifecycleKind.Scoped)]
    public class TenantDependency2
    {
        public ITenant Tenant { get; }

        public int Number { get; }

        public TenantDependency2(ITenant tenant)
        {
            Tenant = tenant;
        }
    }
}