using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Metadata;
using Blacklite.Framework.Multitenancy.Metadata;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyMetadataCollectionExtensions
    {
        public static IServiceCollection AddMultitenancyMetadata(
            [NotNull] this IServiceCollection services,
            )
        {
            services.AddMetadata()
                    .Add(BlackliteMultitenancyMetadataServices.GetMultitenancyMetadata(configuration));
            return services;
        }
    }
}
