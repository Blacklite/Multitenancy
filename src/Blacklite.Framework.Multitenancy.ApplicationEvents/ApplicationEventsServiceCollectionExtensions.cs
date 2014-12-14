using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.ApplicationEvents;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancyApplicationEvents(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.TryAdd(ApplicationEventsServices.GetApplicationEvents(configuration));
            return services;
        }
    }
}
