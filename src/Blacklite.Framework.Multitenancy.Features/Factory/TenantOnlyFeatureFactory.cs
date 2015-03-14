using Blacklite.Framework.Features.Factory;
using System;

namespace Blacklite.Framework.Multitenancy.Features.Factory
{
    public interface ITenantOnlyFeatureFactory : IFeatureFactory { }
    public class TenantOnlyFeatureFactory : ITenantOnlyFeatureFactory
    {
        private readonly IFeatureFactory _factory;

        public TenantOnlyFeatureFactory(
            IFeatureCompositionProvider featureCompositionProvider)
        {
            _factory = new FeatureFactory(featureCompositionProvider);
        }

        public TFeature GetFeature<TFeature>() where TFeature : class, new()
        {
            return _factory.GetFeature<TFeature>();
        }
    }
}
