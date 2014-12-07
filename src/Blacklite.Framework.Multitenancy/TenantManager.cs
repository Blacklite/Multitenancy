using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy
{
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

        public ITenant GetTenant(string tenantId)
        {
            if (_provider.Tenants.Any(z => string.Equals(z, tenantId, StringComparison.OrdinalIgnoreCase)))
            {
                return _provider.GetOrCreateTenant(tenantId).Tenant;
            }
            return null;
        }

        public IEnumerable<string> AvailableTenants
        {
            get
            {
                return _provider.Tenants;
            }
        }

        public void Boot(ITenant tenant)
        {
            ((Tenant)tenant).ExecuteBootOperation(new Operations.BootOperation());
        }

        public void Start(ITenant tenant)
        {
            ((Tenant)tenant).ExecuteStartOperation(new Operations.StartOperation());
        }

        public void Stop(ITenant tenant)
        {
            ((Tenant)tenant).ExecuteStopOperation(new Operations.StopOperation());
        }

        public void Shutdown(ITenant tenant)
        {
            ((Tenant)tenant).ExecuteShutdownOperation(new Operations.ShutdownOperation());
        }
    }
}
