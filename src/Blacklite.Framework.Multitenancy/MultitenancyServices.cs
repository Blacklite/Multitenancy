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

            yield return describe.Singleton<ITenantConfigurationService, TenantConfigurationService>();

            yield return describe.TenantSingleton<ITenantLogger, TenantLogger>();
            yield return describe.TenantSingleton<ITenant, Tenant>();
            yield return describe.TenantSingleton<ITenantConfiguration, TenantConfiguration>();
            yield return describe.TenantSingleton<ILogger>(x => x.GetRequiredService<ITenantLogger>());
        }

        public static IEnumerable<IServiceDescriptor> GetApplicationEvents(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.Singleton<ITenantComposer, ApplicationBroadcastComposer>();
            yield return describe.Singleton<IApplicationObservable, ApplicationObservable>();
            yield return describe.Singleton<IApplicationOrchestrator, ApplicationOrchestrator>();
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

        private static bool IsPerTenant(IServiceDescriptor service)
        {
            return service is TenantServiceDescriptor || (
                        service.ServiceType != null && service.ServiceType.GetTypeInfo().GetCustomAttributes<LifecyclePerTenantAttribute>(true).Any() ||
                        service.ImplementationType != null && service.ImplementationType.GetTypeInfo().GetCustomAttributes<LifecyclePerTenantAttribute>(true).Any()
                   );
        }

        public static bool IsTenantSingleton(IServiceDescriptor service)
        {
            return service.Lifecycle == LifecycleKind.Singleton && IsPerTenant(service);
        }

        public static bool IsNotTenantSingleton(IServiceDescriptor service)
        {
            return !IsTenantSingleton(service);
        }
    }
}
