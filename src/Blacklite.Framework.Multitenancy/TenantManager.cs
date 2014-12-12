using Blacklite.Framework.Multitenancy.Events;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private ITenantProvider _provider;
        public TenantManager(ITenantProvider provider)
        {
            _provider = provider;
        }

        public ITenant GetTenant(string tenantId) { return _provider.GetOrAdd(tenantId)?.Tenant; }

        public IEnumerable<string> AvailableTenants { get { return _provider.Tenants; } }

        public void Boot(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException("All tenant implementations must derive from the \{nameof(Tenant)} class.");

            t.Broadcast(Event.Boot());
        }

        public void Start(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException("All tenant implementations must derive from the \{nameof(Tenant)} class.");

            t.Broadcast(Event.Start());
        }

        public void Stop(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException("All tenant implementations must derive from the \{nameof(Tenant)} class.");

            t.Broadcast(Event.Stop());
        }

        public void Shutdown(ITenant tenant)
        {
            var t = tenant as Tenant;
            if (t == null)
                throw new NotSupportedException("All tenant implementations must derive from the \{nameof(Tenant)} class.");

            t.Broadcast(Event.Shutdown());
        }
    }
}
