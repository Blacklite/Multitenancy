using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.Configuration
{
    public interface ITenantConfigurationService
    {
        void Configure(ITenant tenant);
        IDictionary<string, string> Serialize(ITenant tenant);
        void Deserialize(ITenant tenant, IDictionary<string, string> tenantConfig);
    }

    class TenantConfigurationService : ITenantConfigurationService
    {
        private IEnumerable<ITenantConfiguration> _configurations;
        public TenantConfigurationService(IEnumerable<ITenantConfiguration> configurations)
        {
            _configurations = configurations;
        }

        public void Configure(ITenant tenant)
        {
            foreach (var service in _configurations)
                service.Configure(tenant);
        }

        public void Deserialize(ITenant tenant, IDictionary<string, string> configs)
        {
            foreach (var x in _configurations.Join(configs, x => x.Key, x => x.Key, (a, b) => new
            {
                Service = a,
                Value = b.Value
            }))
            {
                x.Service.Deserialize(tenant, x.Value);
            }
        }

        public IDictionary<string, string> Serialize(ITenant tenant)
        {
            return _configurations
                .Join(tenant.Settings, x => x.Key, x => x.Key, (a, b) => new KeyValuePair<string, string>(a.Serialize(tenant), b.Key))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
