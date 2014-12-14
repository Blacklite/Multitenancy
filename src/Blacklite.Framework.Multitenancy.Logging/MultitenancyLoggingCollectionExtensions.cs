using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class MultitenancyLoggingCollectionExtensions
    {
        public static IServiceCollection AddMultitenancyLogging(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(MultitenancyLoggingServices.GetDefaultServices(configuration));
            return services;
        }
    }
}
