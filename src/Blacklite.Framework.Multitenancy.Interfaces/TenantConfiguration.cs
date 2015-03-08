using Microsoft.Framework.ConfigurationModel;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    [TenantOnly]
    public interface ITenantConfiguration : IConfigurationSourceContainer
    {

    }
}
