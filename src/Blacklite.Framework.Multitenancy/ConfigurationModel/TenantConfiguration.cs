using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    public interface ITenantConfiguration : IConfiguration
    {

    }

    [LifecyclePerTenant]
    public class TenantConfiguration : Configuration, ITenantConfiguration
    {

    }
}
