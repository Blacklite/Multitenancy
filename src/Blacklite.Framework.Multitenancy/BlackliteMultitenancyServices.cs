using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blacklite.Framework.Multitenancy.Configuration;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Blacklite.Framework.Multitenancy.Events;

namespace Blacklite.Framework.Multitenancy
{
    public static class BlackliteMultitenancyServices
    {
        public static IEnumerable<ServiceDescriptor> GetDefaultServices()
        {
            yield return ApplicationOnlyServiceDescriptor.Singleton<ITenantConfigurationService, TenantConfigurationService>();
            yield return ApplicationOnlyServiceDescriptor.Singleton<ITenantRegistry, DefaultTenantRegistry>();
        }

        public static IEnumerable<ServiceDescriptor> GetCollectionServices()
        {
            yield return ServiceDescriptor.Transient<ITenantComposer, EventsBroadcastComposer>();
        }
    }
}
