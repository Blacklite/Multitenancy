using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy
{
    [ApplicationOnly]
    public interface ITenantRegistry
    {
        Task<IEnumerable<ITenantRegistryItem>> GetTenantsAsync();

        IEnumerable<ITenantRegistryItem> GetTenants();

        Task<ITenantRegistryItem> GetTenantItemAsync(string tenantId);

        ITenantRegistryItem GetTenantItem(string tenantId);
    }

    public class DefaultTenantRegistry : ITenantRegistry
    {
        private readonly IDictionary<string, ITenantRegistryItem> _items = new Dictionary<string, ITenantRegistryItem>();

        public ITenantRegistryItem GetTenantItem(string tenantId)
        {
            ITenantRegistryItem item;
            if (!_items.TryGetValue(tenantId, out item))
            {
                item = new TenantRegistryItem(tenantId, true);
                _items.Add(tenantId, item);
            }
            return item;
        }

        public Task<ITenantRegistryItem> GetTenantItemAsync(string tenantId)
        {
            return Task.FromResult(GetTenantItem(tenantId));
        }

        public IEnumerable<ITenantRegistryItem> GetTenants()
        {
            return _items.Values.AsEnumerable();
        }

        public Task<IEnumerable<ITenantRegistryItem>> GetTenantsAsync()
        {
            return Task.FromResult(_items.Values.AsEnumerable());
        }
    }
}
