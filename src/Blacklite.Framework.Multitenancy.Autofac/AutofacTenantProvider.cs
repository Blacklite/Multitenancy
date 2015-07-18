using Autofac;
using Autofac.Core;
using Blacklite.Framework.Multitenancy.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.Configuration;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    public class AutofacTenantProvider : ITenantProvider
    {
        public static string ApplicationTag = "__%Application";
        public static string TenantTag = "__%Tenant";

        private readonly ILifetimeScope _lifetimeScope;
        private readonly ITenantConfigurationService _configurationService;
        private readonly ConcurrentDictionary<string, ITenantScope> _tenantScopes = new ConcurrentDictionary<string, ITenantScope>();

        public AutofacTenantProvider([NotNull] ILifetimeScope lifetimeScope, [NotNull] ITenantConfigurationService configurationService)
        {
            var childLifetimeScope = lifetimeScope as ISharingLifetimeScope;
            if (childLifetimeScope != null)
                _lifetimeScope = childLifetimeScope.RootLifetimeScope;
            else
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
            return _tenantScopes.GetOrAdd(tenantId, x => new TenantScope(_lifetimeScope.BeginLifetimeScope(AutofacTenantProvider.TenantTag, builder => ConfigureTenant(tenantId, builder)), _configurationService, x));
        }

        public void DisposeTenant(string tenantId)
        {
            ITenantScope tenant;
            if (_tenantScopes.TryGetValue(tenantId, out tenant))
            {
                tenant.Dispose();
            }
        }

        private void ConfigureTenant(string tenantId, ContainerBuilder builder)
        {
            var configuration = new TenantConfiguration(Enumerable.Empty<IConfigurationSource>().ToList());
            var tenant = new Tenant(tenantId, configuration);

            builder.RegisterInstance<ITenant>(tenant);
            builder.RegisterInstance(tenant);
        }
    }

    class TenantScope : ITenantScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        public TenantScope([NotNull] ILifetimeScope lifetimeScope, [NotNull] ITenantConfigurationService configurationService, [NotNull] string tenantId)
        {
            _lifetimeScope = lifetimeScope;
            ServiceProvider = _lifetimeScope.Resolve<IServiceProvider>();
            var tenant = _lifetimeScope.Resolve<Tenant>();
            configurationService.Configure(tenant, ServiceProvider);

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
