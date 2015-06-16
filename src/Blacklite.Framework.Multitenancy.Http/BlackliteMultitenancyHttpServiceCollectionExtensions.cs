using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyHttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpMultitenancy([NotNull] this IServiceCollection services)
        {
            services.AddMultitenancy()
                    .TryAdd(BlackliteMultitenancyHttpServices.GetDefaultServices());
            return services;
        }
    }
}
