using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantProvider
    {
        IServiceScope GetOrCreateScope(string tenantId);
    }
}
