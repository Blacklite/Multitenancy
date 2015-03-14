using Blacklite.Framework.Metadata;
using Blacklite.Framework.Metadata.Properties;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.Metadata
{
    public static class BlackliteMultitenancyMetadataServices
    {
        public static IEnumerable<IServiceDescriptor> GetMultitenancyMetadata(IConfiguration configuration = null)
        {
            var describe = new ServiceDescriber(configuration);

            yield return describe.TenantOnlySingleton<ITenantMetadataProvider, TenantMetadataProvider>();
            yield return describe.Scoped<IMetadataProvider, ScopedMetadataProvider>();
        }
    }
}
