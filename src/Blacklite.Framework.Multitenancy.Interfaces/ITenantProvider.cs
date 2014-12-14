using Microsoft.Framework.Runtime;
using System;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantProvider
    {
        ITenantScope Get([NotNull] string tenantId);
        ITenantScope GetOrAdd([NotNull] string tenantId);
        void DisposeTenant(string tenantId);
        IEnumerable<string> Tenants { get; }
    }
}
