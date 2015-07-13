using Blacklite.Framework.Metadata;
using Blacklite.Framework.Metadata.Properties;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.Metadata
{
    public static class BlackliteMultitenancyMetadataServices
    {
        public static IEnumerable<ServiceDescriptor> GetMultitenancyMetadata()
        {
            yield return TenantOnlyServiceDescriptor.Singleton<ITenantMetadataProvider, TenantMetadataProvider>();
            yield return ServiceDescriptor.Scoped<IMetadataProvider, ScopedMetadataProvider>();
        }
    }
}
