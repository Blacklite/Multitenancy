using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    [AssemblyNeutral]
    [TenantOnly]
    public interface ITenantConfiguration : IConfiguration
    {

    }
}
