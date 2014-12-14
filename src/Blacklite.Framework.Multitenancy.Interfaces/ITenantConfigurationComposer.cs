using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface ITenantConfigurationComposer
    {
        string Key { get; }
        int Order { get; }
        void Configure(ITenant tenant, IConfiguration configuration);
    }
}
