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
            yield return TenantOnlyServiceDescriptor.Singleton<ITenantLogger, TenantLogger>();
            yield return ApplicationOnlyServiceDescriptor.Singleton<IApplicationLogger, ApplicationLogger>();
        }
    }
}
