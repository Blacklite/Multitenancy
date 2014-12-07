using Autofac;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    class TenantProvider : ITenantProvider
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, ITenantScope> _tenantScopes = new ConcurrentDictionary<string, ITenantScope>();

        public TenantProvider([NotNull] ILifetimeScope lifetimeScope, [NotNull] IServiceProvider serviceProvider)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = serviceProvider;
        }

        public ITenantScope GetOrCreateTenant(string tenantId)
        {
            return _tenantScopes.GetOrAdd(tenantId, x => new TenantScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantRegistration.TenantTag), x));
        }

        public IEnumerable<string> Tenants { get { return _tenantScopes.Keys; } }

        public void DisposeTenant(string tenantId)
        {
            ITenantScope tenant;
            if (_tenantScopes.TryGetValue(tenantId, out tenant))
            {
                tenant.Dispose();
            }
        }
    }

    class TenantScope : ITenantScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        public TenantScope([NotNull] ILifetimeScope lifetimeScope, [NotNull] string tenantId)
        {
            _lifetimeScope = lifetimeScope;
            ServiceProvider = _lifetimeScope.Resolve<IServiceProvider>();

            Tenant = ServiceProvider.GetRequiredService<ITenant>();
            Tenant.Initialize(tenantId);
        }

        public IServiceProvider ServiceProvider { get; }

        public ITenant Tenant { get; }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
