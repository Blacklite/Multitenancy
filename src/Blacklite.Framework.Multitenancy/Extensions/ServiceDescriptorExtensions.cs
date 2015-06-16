using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy
{
    public static class ServiceDescriptorExtensions
    {
        public static bool IsTenantScope(this ServiceDescriptor service)
        {
            return service is TenantOnlyServiceDescriptor || (
                          service.ServiceType != null && service.ServiceType.GetTypeInfo().GetCustomAttributes<TenantOnlyAttribute>(true).Any() ||
                          service.ImplementationType != null && service.ImplementationType.GetTypeInfo().GetCustomAttributes<TenantOnlyAttribute>(true).Any()
                     );
        }

        public static bool IsApplicationScope(this ServiceDescriptor service)
        {
            return service is ApplicationOnlyServiceDescriptor || (
                           service.ServiceType != null && service.ServiceType.GetTypeInfo().GetCustomAttributes<ApplicationOnlyAttribute>(true).Any() ||
                           service.ImplementationType != null && service.ImplementationType.GetTypeInfo().GetCustomAttributes<ApplicationOnlyAttribute>(true).Any()
                      );
        }
    }
}
