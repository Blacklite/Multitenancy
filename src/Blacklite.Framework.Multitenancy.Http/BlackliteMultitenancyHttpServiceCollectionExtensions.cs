using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyHttpServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(BlackliteMultitenancyServices.GetDefaultServices(configuration));
            services.TryAdd(BlackliteMultitenancyHttpServices.GetDefaultServices(configuration));
            return services;
        }
    }
}
