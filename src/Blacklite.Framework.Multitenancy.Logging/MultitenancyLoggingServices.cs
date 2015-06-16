using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy
{
    public static class MultitenancyLoggingServices
    {
        public static IEnumerable<ServiceDescriptor> GetDefaultServices()
        {
            yield return describe.TenantOnlySingleton<ITenantLogger, TenantLogger>();
            yield return describe.TenantOnlySingleton<ILogger>(x => x.GetRequiredService<ITenantLogger>());
        }
    }
}
