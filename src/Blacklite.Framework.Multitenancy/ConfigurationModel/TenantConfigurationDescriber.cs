using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    public interface ITenantConfigurationDescriber
    {
        string Key { get; }
        int Order { get; }
        void Configure(ITenant tenant);
    }
}
