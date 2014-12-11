using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
    }

    class TenantConfigurationService : ITenantConfigurationService
    {
        private IEnumerable<ITenantComposer> _tenantComposers;
        private IEnumerable<ITenantConfigurationComposer> _tenantConfigurationComposers;
        public TenantConfigurationService(IEnumerable<ITenantComposer> tenantComposers, IEnumerable<ITenantConfigurationComposer> tenantConfigurationComposers)
        {
            _tenantComposers = tenantComposers
                .OrderByDescending(z => z.Order);

            _tenantConfigurationComposers = tenantConfigurationComposers
                .OrderByDescending(z => z.Order);
        }

        public void Configure(ITenant tenant)
        {
            foreach (var service in _tenantConfigurationComposers)
                service.Configure(tenant, tenant.Configuration.GetSubKey(service.Key));

            foreach (var service in _tenantComposers)
                service.Configure(tenant);
        }
    }
}
