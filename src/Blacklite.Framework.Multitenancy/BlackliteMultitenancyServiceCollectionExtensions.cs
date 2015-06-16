using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class BlackliteMultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMultitenancy([NotNull] this IServiceCollection services)
        {
            services.TryAdd(BlackliteMultitenancyServices.GetDefaultServices());
            return services;
        }
    }
}
