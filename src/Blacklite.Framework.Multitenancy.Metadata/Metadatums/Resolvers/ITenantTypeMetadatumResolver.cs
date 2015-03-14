using Blacklite.Framework.Metadata;
using Blacklite.Framework.Metadata.Metadatums;
using Blacklite.Framework.Metadata.Metadatums.Resolvers;
using System;

namespace Blacklite.Framework.Multitenancy.Metadata.Metadatums.Resolvers
{
    public interface ITenantTypeMetadatumResolver : IMetadatumResolver<ITypeMetadata>, IMetadatumResolver { }

    public interface ITenantTypeMetadatumResolver<TMetadatum> : IMetadatumResolver<ITypeMetadata, TMetadatum>, IApplicationTypeMetadatumResolver where TMetadatum : IMetadatum { }

}
