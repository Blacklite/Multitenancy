using Blacklite.Framework.Metadata;
using Blacklite.Framework.Metadata.Metadatums;
using Blacklite.Framework.Metadata.Metadatums.Resolvers;
using System;

namespace Blacklite.Framework.Multitenancy.Metadata.Metadatums.Resolvers
{
    public interface ITenantPropertyMetadatumResolver : IMetadatumResolver<IPropertyMetadata>, IMetadatumResolver { }
    
    public interface ITenantPropertyMetadatumResolver<TMetadatum> : IMetadatumResolver<IPropertyMetadata, TMetadatum>, IApplicationPropertyMetadatumResolver where TMetadatum : IMetadatum { }
}
