#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [ApplicationOnly]
    public interface ITenantConfigurationComposer
    {
        string Key { get; }
        int Order { get; }
        void Configure(ITenant tenant, IConfiguration configuration);
    }
}
