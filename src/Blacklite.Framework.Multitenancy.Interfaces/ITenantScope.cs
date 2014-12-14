using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantScope : IDisposable
    {
        ITenant Tenant { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
