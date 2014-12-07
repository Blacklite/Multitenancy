using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy
{
    public static class MultitenancyServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.Singleton<ITenantLogger, TenantLogger>();
            yield return describe.Singleton<ITenant, Tenant>();
        }

        public static bool HasRequiredServicesRegistered(IServiceCollection services)
        {
            if (!services.Any(z => z.ServiceType == typeof(ITenant)))
                throw new Exception("\{nameof(ITenant)} has not been registered, \{nameof(ITenant)} is required for Multitenancy.");
            if (!services.Any(z => z.ServiceType == typeof(ITenantProvider)))
                throw new Exception("\{nameof(ITenantProvider)} has not been registered, \{nameof(ITenantProvider)} is required for Multitenancy.");
            if (!services.Any(z => z.ServiceType == typeof(ITenantIdentificationStrategy)))
                throw new Exception("\{nameof(ITenantIdentificationStrategy)} has not been registered, \{nameof(ITenantIdentificationStrategy)} is required for Multitenancy.");

            return true;
        }
    }
}
