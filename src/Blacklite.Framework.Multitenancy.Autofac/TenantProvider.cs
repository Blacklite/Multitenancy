using Autofac;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
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
        private readonly ITenantConfigurationService _configurationService;
        private readonly ConcurrentDictionary<string, ITenantScope> _tenantScopes = new ConcurrentDictionary<string, ITenantScope>();

        public TenantProvider([NotNull] ILifetimeScope lifetimeScope, [NotNull] ITenantConfigurationService configurationService, [NotNull] IServiceProvider serviceProvider)
        {
            _lifetimeScope = lifetimeScope;
            _serviceProvider = serviceProvider;
            _configurationService = configurationService;
        }

        public ITenantScope GetOrCreateTenant(string tenantId)
        {
            return _tenantScopes.GetOrAdd(tenantId, x => new TenantScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantRegistration.TenantTag), _configurationService, x));
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

        public TenantScope([NotNull] ILifetimeScope lifetimeScope, [NotNull] ITenantConfigurationService configurationService, [NotNull] string tenantId)
        {
            _lifetimeScope = lifetimeScope;
            ServiceProvider = _lifetimeScope.Resolve<IServiceProvider>();

            Tenant = ServiceProvider.GetRequiredService<ITenant>();
            Tenant.Initialize(tenantId);
            configurationService.Configure(Tenant);
        }

        public IServiceProvider ServiceProvider { get; }

        public ITenant Tenant { get; }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
