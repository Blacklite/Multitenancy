using Blacklite.Framework.Features;
using Blacklite.Framework.Features.Describers;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.Features.Describers
{
    public class MultitenancyFeatureDescriberFactory : IFeatureDescriberFactory
    {
        public IEnumerable<IFeatureDescriber> Create(IEnumerable<TypeInfo> descriptors)
        {
            return Fixup(descriptors.Select(x => new MultitenancyFeatureDescriber(x)));
        }

        private IEnumerable<MultitenancyFeatureDescriber> Fixup(IEnumerable<MultitenancyFeatureDescriber> describers)
        {
            foreach (var describer in describers)
            {
                describer.Children = describers.Where(x => x.Parent == describer.TypeInfo).ToArray();

                var requires = describers
                    .Join(describer.Requires, x => x.TypeInfo, x => x.FeatureType, (z, x) => z);
                var requiresDictionary = requires.Join(describer.Requires, x => x.TypeInfo,
                    x => x.FeatureType, (d, x) => new { d, x.IsEnabled }).ToDictionary(x => (IFeatureDescriber)x.d, x => x.IsEnabled);

                describer.DependsOn = new ReadOnlyDictionary<IFeatureDescriber, bool>(requiresDictionary);

                ValidateMultitenancyDescriber(describer);

                yield return describer;
            }
        }

        internal static void ValidateMultitenancyDescriber<T>(T describer)
            where T : MultitenancyFeatureDescriber
        {
            FeatureDescriberFactory.ValidateDescriber(describer);
            var requires = describer.DependsOn.Keys.Cast<T>();

            if (describer.Lifecycle == LifecycleKind.Singleton && !(describer.IsTenantScoped || describer.IsApplicationScoped) && requires.Any(z => z.IsTenantScoped))
            {
                throw new NotSupportedException($"Lifecycle '{nameof(Tenant)}' cannot be required by features with a lifecycle of '{describer.Lifecycle}'.");
            }
            if (describer.Lifecycle == LifecycleKind.Singleton && !(describer.IsTenantScoped || describer.IsApplicationScoped) && requires.Any(z => z.IsApplicationScoped))
            {
                throw new NotSupportedException($"Lifecycle 'Application' cannot be required by features with a lifecycle of '{describer.Lifecycle}'.");
            }
        }
    }
}
