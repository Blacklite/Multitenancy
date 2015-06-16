using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Tenants.Tests.Web
{
    [TenantOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Singleton)]
    public class TenantDependencySingleton
    {
        public ITenant Tenant { get; }

        public int Number { get; set; }

        public TenantDependencySingleton(ITenant tenant)
        {
            Tenant = tenant;
        }
    }

    [TenantOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Scoped)]
    public class TenantDependencyScoped
    {
        public ITenant Tenant { get; }

        public int Number { get; set; }

        public TenantDependencyScoped(ITenant tenant)
        {
            Tenant = tenant;
        }
    }

    [TenantOnly]
    [ServiceDescriptor(Lifetime = ServiceLifetime.Transient)]
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
