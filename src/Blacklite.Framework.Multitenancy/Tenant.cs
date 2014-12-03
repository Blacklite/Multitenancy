using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenant
    {
        string Identifier { get; set; }
        object this[string key] { get; set; }
        IReadOnlyDictionary<string, object> Settings { get; }
    }

    public class Tenant : ITenant
    {
        private IDictionary<string, object> _settings = new Dictionary<string, object>();
        private string _identifier;

        public Tenant()
        {
            Settings = new ReadOnlyDictionary<string, object>(_settings);
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                if (_identifier == null)
                    _identifier = value;
            }
        }

        public object this[string key]
        {
            get
            {
                object value;
                if (Settings.TryGetValue(key, out value))
                    return value;
                return null;
            }
            set
            {
                lock (_settings)
                {
                    if (_settings.ContainsKey(key))
                    {
                        _settings.Remove(key);
                    }
                    _settings.Add(key, value);
                }
            }
        }
        
        public IReadOnlyDictionary<string, object> Settings { get; }
    }
}