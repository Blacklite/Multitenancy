using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantProvider
    {
        ITenantScope GetOrCreateTenant([NotNull] string tenantId);
        void DisposeTenant(string tenantId);
        IEnumerable<string> Tenants { get; }
    }
}
