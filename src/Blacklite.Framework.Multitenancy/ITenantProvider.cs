using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantProvider
    {
        ITenantScope Get([NotNull] string tenantId);
        ITenantScope GetOrAdd([NotNull] string tenantId);
        void DisposeTenant(string tenantId);
        IEnumerable<string> Tenants { get; }
    }
}
