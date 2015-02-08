using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy
{
    public static class BlackliteMultitenancyServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.ApplicationOnlySingleton<ITenantConfigurationService, TenantConfigurationService>();

            yield return describe.TenantOnlySingleton<ITenant, Tenant>();
            yield return describe.TenantOnlySingleton<ITenantConfiguration, TenantConfiguration>();
            yield return describe.ApplicationOnlySingleton<ITenantRegistry, DefaultTenantRegistry>();
        }
    }
}
