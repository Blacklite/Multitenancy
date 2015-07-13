using Blacklite.Framework.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

namespace Blacklite.Framework.Multitenancy.Events
{
    public class TenantEvent : Event, ITenantEvent
    {
        public TenantEvent(IDictionary<string, object> data = null) : base(data) { }

        public string Tenant { get { return _getValue<string>(nameof(ITenantEvent.Tenant)); } }

        public static TenantEvent Create(string tenantId, IEvent @event)
        {
            var dict = @event.Data.ToDictionary(x => x.Key, x => x.Value);
            dict[nameof(ITenantEvent.Tenant)] = tenantId;
            return new TenantEvent(dict);
        }

        public static Event Boot()
        {
            return new Event(new Dictionary<string, object>() {
                { nameof(Event.Category), nameof(Tenant) },
                { nameof(Event.Type), TenantState.Boot.ToString() },
            });
        }

        public static Event Start()
        {
            return new Event(new Dictionary<string, object>() {
                { nameof(Event.Category), nameof(Tenant) },
                { nameof(Event.Type), TenantState.Started.ToString() },
            });
        }

        public static Event Stop()
        {
            return new Event(new Dictionary<string, object>() {
                { nameof(Event.Category), nameof(Tenant) },
                { nameof(Event.Type), TenantState.Stopped.ToString() },
            });
        }

        public static Event Shutdown()
        {
            return new Event(new Dictionary<string, object>() {
                { nameof(Event.Category), nameof(Tenant) },
                { nameof(Event.Type), TenantState.Shutdown.ToString() },
            });
        }
    }
}
