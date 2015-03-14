using Blacklite.Framework.Features.Describers;
using Blacklite.Framework.Features.Factory;
using Blacklite.Framework.Multitenancy.Features;
using Blacklite.Framework.Multitenancy.Features.Describers;
using Blacklite.Framework.Multitenancy.Features.Factory;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Features
{
    public static class BlackliteMultitenancyFeaturesServices
    {
        internal static IEnumerable<IServiceDescriptor> GetMultitenancyFeatures(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.Singleton<IFeatureDescriberFactory, MultitenancyFeatureDescriberFactory>();
            yield return describe.Singleton<IApplicationOnlyFeatureFactory, ApplicationOnlyFeatureFactory>();
            yield return describe.Singleton<ITenantOnlyFeatureFactory, TenantOnlyFeatureFactory>();
            yield return describe.Singleton<IFeatureFactory, MultitenancyCompositeFeatureFactory>();
        }
    }
}
