using Blacklite.Framework.Multitenancy.ApplicationEvents;
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
    public static class MultitenancyServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.ApplicationOnlySingleton<ITenantConfigurationService, TenantConfigurationService>();

            yield return describe.TenantOnlySingleton<ITenantLogger, TenantLogger>();
            yield return describe.TenantOnlySingleton<ITenant, Tenant>();
            yield return describe.TenantOnlySingleton<ITenantConfiguration, TenantConfiguration>();
            yield return describe.TenantOnlySingleton<ILogger>(x => x.GetRequiredService<ITenantLogger>());
        }

        public static IEnumerable<IServiceDescriptor> GetApplicationEvents(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.ApplicationOnlySingleton<ITenantComposer, ApplicationBroadcastComposer>();
            yield return describe.ApplicationOnlySingleton<IApplicationObservable, ApplicationObservable>();
            yield return describe.ApplicationOnlySingleton<IApplicationOrchestrator, ApplicationOrchestrator>();
        }

        public static bool HasRequiredServicesRegistered(IServiceCollection services)
        {
            if (!services.Any(z => z.ServiceType == typeof(ITenant)))
                throw new Exception(string.Format("{0} has not been registered, {0} is required for Multitenancy.", nameof(ITenant)));

            if (!services.Any(z => z.ServiceType == typeof(ITenantProvider)))
                throw new Exception(string.Format("{0} has not been registered, {0} is required for Multitenancy.", nameof(ITenantProvider)));

            if (!services.Any(z => z.ServiceType == typeof(ITenantIdentificationStrategy)))
                throw new Exception(string.Format("{0} has not been registered, {0} is required for Multitenancy.", nameof(ITenantIdentificationStrategy)));

            return true;
        }

        public static bool IsTenantScope(this IServiceDescriptor service)
        {
            return service is TenantOnlyServiceDescriptor || (
                          service.ServiceType != null && service.ServiceType.GetTypeInfo().GetCustomAttributes<TenantOnlyAttribute>(true).Any() ||
                          service.ImplementationType != null && service.ImplementationType.GetTypeInfo().GetCustomAttributes<TenantOnlyAttribute>(true).Any()
                     );
        }

        public static bool IsApplicationScope(this IServiceDescriptor service)
        {
            return service is ApplicationOnlyServiceDescriptor || (
                           service.ServiceType != null && service.ServiceType.GetTypeInfo().GetCustomAttributes<ApplicationOnlyAttribute>(true).Any() ||
                           service.ImplementationType != null && service.ImplementationType.GetTypeInfo().GetCustomAttributes<ApplicationOnlyAttribute>(true).Any()
                      );
        }
    }
}
