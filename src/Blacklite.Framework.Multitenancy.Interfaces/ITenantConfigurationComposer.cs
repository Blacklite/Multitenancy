using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [ApplicationOnly]
    public interface ITenantConfigurationComposer
    {
        string Key { get; }
        int Order { get; }
        void Configure(ITenant tenant, IConfiguration configuration);
    }
}
