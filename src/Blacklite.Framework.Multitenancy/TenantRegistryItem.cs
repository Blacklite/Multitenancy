using System;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantRegistryItem
    {
        string Id { get; }
        bool IsEnabled { get; }
    }

    public class TenantRegistryItem : ITenantRegistryItem
    {
        public TenantRegistryItem(string id, bool enabled)
        {
            Id = id;
            IsEnabled = enabled;
        }

        public string Id { get; }
        public bool IsEnabled { get; }
    }
}
