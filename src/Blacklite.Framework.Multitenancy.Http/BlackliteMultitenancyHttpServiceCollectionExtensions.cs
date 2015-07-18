using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Hosting.Internal;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyHttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpMultitenancy([NotNull] this IServiceCollection services)
        {
            services.AddMultitenancy()
                    .TryAdd(BlackliteMultitenancyHttpServices.GetDefaultServices());

            var autoRequestServices = services.FirstOrDefault(x => x.ImplementationType == typeof(AutoRequestServicesStartupFilter));
            if (autoRequestServices != null)
            {
                services.Remove(autoRequestServices);
            }

            return services;
        }
    }
}
