using Blacklite.Framework.Features.Factory;
using System;

namespace Blacklite.Framework.Multitenancy.Features.Factory
{
    public interface IApplicationOnlyFeatureFactory : IFeatureFactory { }
    public class ApplicationOnlyFeatureFactory : IApplicationOnlyFeatureFactory
    {
        private readonly IFeatureFactory _factory;

        public ApplicationOnlyFeatureFactory(
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
