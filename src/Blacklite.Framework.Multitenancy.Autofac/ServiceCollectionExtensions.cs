using Autofac;
using Blacklite;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Autofac;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutofacMultitenancy(
            [NotNull] this IServiceCollection collection,
            IConfiguration configuration = null)
        {
            collection.AddMultitenancy(configuration);
            return collection;
        }
    }
}
