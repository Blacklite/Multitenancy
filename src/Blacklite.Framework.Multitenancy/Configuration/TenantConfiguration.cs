using System;

namespace Blacklite.Framework.Multitenancy.Configuration
{
    public interface ITenantConfiguration
    {
        string Key { get; }
        void Configure(ITenant tenant);
        string Serialize(ITenant tenant);
        void Deserialize(ITenant tenant, string Config);
    }
}
