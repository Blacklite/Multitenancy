#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using System;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [ApplicationOnly]
    public interface ITenantProvider
    {
        ITenantScope Get([NotNull] string tenantId);
        ITenantScope GetOrAdd([NotNull] string tenantId);
        void DisposeTenant(string tenantId);
        IEnumerable<string> Tenants { get; }
    }
}
