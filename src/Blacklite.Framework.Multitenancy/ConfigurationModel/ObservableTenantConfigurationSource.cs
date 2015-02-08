using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.ConfigurationModel
{
    class ObservableTenantConfigurationSource : IConfigurationSource
    {
        private readonly IObserver<KeyValuePair<string, string>> _observer;
        public ObservableTenantConfigurationSource(IObserver<KeyValuePair<string, string>> observer)
        {
            _observer = observer;
        }
        public void Load() { }

        public IEnumerable<string> ProduceSubKeys(IEnumerable<string> earlierKeys, string prefix, string delimiter)
        {
            return Enumerable.Empty<string>();
        }

        public void Set(string key, string value)
        {
            _observer.OnNext(new KeyValuePair<string, string>(key, value));
        }

        public bool TryGet(string key, out string value)
        {
            value = null;
            return false;
        }
    }
}
