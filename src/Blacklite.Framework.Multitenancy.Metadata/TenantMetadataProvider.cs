using Blacklite.Framework.Metadata.Properties;
using Blacklite.Framework.Metadata.Metadatums;
using Blacklite.Framework.Metadata.Metadatums.Resolvers;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using Blacklite.Framework.Metadata;

namespace Blacklite.Framework.Multitenancy.Metadata
{
    public interface ITenantMetadataProvider : IMetadataProvider { }

    class TenantMetadataProvider : ITenantMetadataProvider
    {
        private readonly IPropertyMetadataProvider _metadataPropertyProvider;
        private readonly IMetadatumResolverProvider _metadatumResolverProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeMetadataFactory _typeMetadataActivator;
        private readonly IApplicationMetadataProvider _metadataProvider;
        private readonly ConcurrentDictionary<Type, ITypeMetadata> _metadata = new ConcurrentDictionary<Type, ITypeMetadata>();

        public TenantMetadataProvider(IApplicationMetadataProvider metadataProvider, ITypeMetadataFactory typeMetadataActivator, IServiceProvider serviceProvider, IPropertyMetadataProvider metadataPropertyProvider, IMetadatumResolverProvider metadatumResolverProvider)
        {
            _metadataPropertyProvider = metadataPropertyProvider;
            _typeMetadataActivator = typeMetadataActivator;
            _metadatumResolverProvider = metadatumResolverProvider;
            _serviceProvider = serviceProvider;
            _metadataProvider = metadataProvider;
        }

        public ITypeMetadata GetMetadata(TypeInfo typeInfo) => GetUnderlyingMetadata(typeInfo.AsType());

        public ITypeMetadata GetMetadata(Type type) => GetUnderlyingMetadata(type);

        public ITypeMetadata GetMetadata<T>() => GetUnderlyingMetadata(typeof(T));

        private ITypeMetadata GetUnderlyingMetadata(Type type) => _metadata.GetOrAdd(type, x => CreateTypeMetadata(x));

        private ITypeMetadata CreateTypeMetadata(Type type) => _typeMetadataActivator.Create(_metadataProvider.GetMetadata(type), "Tenant", _serviceProvider, _metadataPropertyProvider, _metadatumResolverProvider);
    }

    class ScopedMetadataProvider : IMetadataProvider
    {
        private readonly IPropertyMetadataProvider _metadataPropertyProvider;
        private readonly IMetadatumResolverProvider _metadatumResolverProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeMetadataFactory _typeMetadataActivator;
        private readonly ITenantMetadataProvider _tenantMetadataProvider;
        private readonly IApplicationMetadataProvider _applicationMetadataProvider;
        private readonly ConcurrentDictionary<Type, ITypeMetadata> _metadata = new ConcurrentDictionary<Type, ITypeMetadata>();

        public ScopedMetadataProvider(ITenantMetadataProvider tenantMetadataProvider, IApplicationMetadataProvider applicationMetadataProvider, ITypeMetadataFactory typeMetadataActivator, IServiceProvider serviceProvider, IPropertyMetadataProvider metadataPropertyProvider, IMetadatumResolverProvider metadatumResolverProvider)
        {
            _metadataPropertyProvider = metadataPropertyProvider;
            _typeMetadataActivator = typeMetadataActivator;
            _metadatumResolverProvider = metadatumResolverProvider;
            _serviceProvider = serviceProvider;
            _tenantMetadataProvider = tenantMetadataProvider;
            _applicationMetadataProvider = applicationMetadataProvider;
        }

        public ITypeMetadata GetMetadata(TypeInfo typeInfo) => GetUnderlyingMetadata(typeInfo.AsType());

        public ITypeMetadata GetMetadata(Type type) => GetUnderlyingMetadata(type);

        public ITypeMetadata GetMetadata<T>() => GetUnderlyingMetadata(typeof(T));

        private ITypeMetadata GetUnderlyingMetadata(Type type) => _metadata.GetOrAdd(type, x => CreateTypeMetadata(x));

        private ITypeMetadata CreateTypeMetadata(Type type)
        {
            var metadata = _tenantMetadataProvider?.GetMetadata(type);
            if (metadata == null)
                metadata = _applicationMetadataProvider.GetMetadata(type);

            return _typeMetadataActivator.Create(metadata, "Scoped", _serviceProvider, _metadataPropertyProvider, _metadatumResolverProvider);
        }
    }
}
