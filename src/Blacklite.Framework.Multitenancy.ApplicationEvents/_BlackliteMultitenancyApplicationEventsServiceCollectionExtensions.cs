using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.ApplicationEvents;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyApplicationEventsServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancyApplicationEvents(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            ConfigureDefaultServices(services, configuration);
            services.Add(BlackliteMultitenancyApplicationEventsServices.GetApplicationEvents(configuration));
            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddGlobalEvents(configuration);
        }
    }
}
