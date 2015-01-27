using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyHttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpMultitenancy(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.AddMultitenancy()
                    .TryAdd(BlackliteMultitenancyHttpServices.GetDefaultServices(configuration));
            return services;
        }
    }
}
