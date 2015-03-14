using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Features;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyFeaturesCollectionExtensions
    {
        public static IServiceCollection AddMultitenancyFeatures(
            [NotNull] this IServiceCollection services,
            IConfiguration configuration = null)
        {
            services.AddFeatures()
                    .Add(BlackliteMultitenancyFeaturesServices.GetMultitenancyFeatures(configuration));

            return services;
        }
    }
}
