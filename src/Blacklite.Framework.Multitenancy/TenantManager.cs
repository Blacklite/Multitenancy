using Blacklite.Framework.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using Blacklite.Framework.Multitenancy.Events;

namespace Blacklite.Framework.Multitenancy
{
    [ApplicationOnly]
    public interface ITenantManager
    {
        ITenant GetTenant(string tenantId);
        IEnumerable<string> AvailableTenants { get; }
        void Boot(ITenant tenant);
        void Start(ITenant tenant);
        void Stop(ITenant tenant);
        void Shutdown(ITenant tenant);
    }

    public class TenantManager : ITenantManager
    {
        private readonly ITenantProvider _provider;
        private readonly ITenantRegistry _registry;
        public TenantManager(ITenantProvider provider, ITenantRegistry registry)
        {
            _provider = provider;
            _registry = registry;
        }

        public ITenant GetTenant(string tenantId) { return _provider.GetOrAdd(tenantId)?.Tenant; }

        public IEnumerable<string> AvailableTenants { get { return _registry.GetTenants().Select(x => x.Id); } }

        public void Boot(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException(string.Format("All tenant implementations must derive from the {0} class.", nameof(Tenant)));

            t.Broadcast(TenantEvent.Boot());
        }

        public void Start(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException(string.Format("All tenant implementations must derive from the {0} class.", nameof(Tenant)));

            t.Broadcast(TenantEvent.Start());
        }

        public void Stop(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException(string.Format("All tenant implementations must derive from the {0} class.", nameof(Tenant)));

            t.Broadcast(TenantEvent.Stop());
        }

        public void Shutdown(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException(string.Format("All tenant implementations must derive from the {0} class.", nameof(Tenant)));

            t.Broadcast(TenantEvent.Shutdown());
        }
    }
}
