using System;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantScope : IDisposable
    {
        ITenant Tenant { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
