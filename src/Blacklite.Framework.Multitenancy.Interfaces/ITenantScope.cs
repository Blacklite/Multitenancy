#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using System;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [ApplicationOnly]
    public interface ITenantScope : IDisposable
    {
        ITenant Tenant { get; }
        IServiceProvider ServiceProvider { get; }
    }
}
