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
        private IEnumerable<ITenantConfigurationDescriber> _configurationDescribers;
        public TenantConfigurationService(IEnumerable<ITenantConfigurationDescriber> configurationDescribers)
        {
            _configurationDescribers = configurationDescribers.OrderByDescending(z => z.Order);
        }

        public void Configure(ITenant tenant)
        {
            foreach (var service in _configurationDescribers)
                service.Configure(tenant);
        }
    }
}
