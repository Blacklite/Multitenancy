using System;

namespace Blacklite.Framework.Multitenancy
{
    [ApplicationOnly]
    public interface ITenantScope : IDisposable
    {
        ITenant Tenant { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
