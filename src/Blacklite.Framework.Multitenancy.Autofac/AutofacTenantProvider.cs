using Autofac;
using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    public class AutofacTenantProvider : ITenantProvider
    {
        public static object Tag = "Tenant";

        private readonly ILifetimeScope _lifetimeScope;
        private readonly ITenantConfigurationService _configurationService;
        private readonly ConcurrentDictionary<string, ITenantScope> _tenantScopes = new ConcurrentDictionary<string, ITenantScope>();

        public AutofacTenantProvider([NotNull] ILifetimeScope lifetimeScope, [NotNull] ITenantConfigurationService configurationService)
        {
            _lifetimeScope = lifetimeScope;
            _configurationService = configurationService;
        }

        public ITenantScope Get([NotNull]string tenantId)
        {
            ITenantScope scope;
            if (_tenantScopes.TryGetValue(tenantId, out scope))
                return scope;
            return null;
        }

        public ITenantScope GetOrAdd(string tenantId)
        {
            return _tenantScopes.GetOrAdd(tenantId, x => new TenantScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantProvider.Tag), _configurationService, x));
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

            var tenant = (Tenant)ServiceProvider.GetRequiredService<ITenant>();
            tenant.Initialize(tenantId);

            configurationService.Configure(Tenant);

            tenant.ChangeState(TenantState.Boot);

            Tenant = tenant;
        }

        public IServiceProvider ServiceProvider { get; }

        public ITenant Tenant { get; }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }
}
