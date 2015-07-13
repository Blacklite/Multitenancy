using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy.Configuration
{
    [TenantOnly]
    public interface ITenantConfiguration : IConfiguration
    {
        IObservable<KeyValuePair<string, string>> Observable { get; }
        IObserver<KeyValuePair<string, string>> Observer { get; }
    }

    public class TenantConfiguration : ConfigurationSection, ITenantConfiguration
    {
        private readonly Subject<KeyValuePair<string, string>> _subject = new Subject<KeyValuePair<string, string>>();

        public TenantConfiguration(IList<IConfigurationSource> sources) : base(sources)
        {
            sources.Add(new ObservableTenantConfigurationSource(_subject));
        }

        public IObservable<KeyValuePair<string, string>> Observable { get { return _subject; } }
        public IObserver<KeyValuePair<string, string>> Observer { get { return _subject; } }
    }
}
