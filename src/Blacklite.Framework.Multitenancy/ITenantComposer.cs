using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantComposer
    {
        int Order { get; }
        void Configure(ITenant tenant);
    }

    public interface ITenantConfigurationComposer
    {
        string Key { get; }
        int Order { get; }
        void Configure(ITenant tenant, IConfiguration configuration);
    }
}
