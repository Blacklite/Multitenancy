using Blacklite.Framework.Metadata;
using Blacklite.Framework.Metadata.Metadatums.Resolvers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.Metadata.Metadatums.Resolvers
{
    class TenantMetadatumResolverProviderCollector : IMetadatumResolverProviderCollector
    {
        public TenantMetadatumResolverProviderCollector(
            IEnumerable<ITenantTypeMetadatumResolver> typeMetadatumResolvers,
            IEnumerable<ITenantPropertyMetadatumResolver> propertyMetadatumResolvers)
        {
            TypeResolvers = MetadatumResolverProviderCollectorHelper.GetMetadatumResolverDictionary<ITenantTypeMetadatumResolver, ITypeMetadata>(typeMetadatumResolvers);
            PropertyResolvers = MetadatumResolverProviderCollectorHelper.GetMetadatumResolverDictionary<ITenantPropertyMetadatumResolver, IPropertyMetadata>(propertyMetadatumResolvers);
        }

        public string Key { get; } = nameof(Tenant);

        public IMetadatumResolverProviderCollectorItem<IPropertyMetadata> PropertyResolvers { get; }

        public IMetadatumResolverProviderCollectorItem<ITypeMetadata> TypeResolvers { get; }
    }
}