using Autofac;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    class TenantProvider : ITenantProvider
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, ITenantScope> _tenantScopes = new ConcurrentDictionary<string, ITenantScope>();

        public TenantProvider(ILifetimeScope lifetimeScope, IServiceProvider serviceProvider)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = serviceProvider;
        }

        public void DisposeTenant(string tenantId)
        {
            ITenantScope tenant;
            if (_tenantScopes.TryGetValue(tenantId, out tenant))
            {
                tenant.Dispose();
            }
        }

        public ITenantScope GetOrCreateTenant(string tenantId)
        {
            return _tenantScopes.GetOrAdd(tenantId, x => new TenantScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantRegistration.TenantTag), x));
        }
    }

    class TenantScope : ITenantScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        public TenantScope(ILifetimeScope lifetimeScope, string tenantId)
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
