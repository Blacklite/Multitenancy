using Blacklite.Framework.Events;
using System;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy.Events
{
    public class TenantEvent : IEvent
    {
        public TenantEvent()
        {

        }

        internal TenantEvent(IEvent @event)
        {
            Type = @event.Type;
            Name = @event.Name;
            User = @event.User;
            Reason = @event.Reason;
            Data = @event.Data ?? Data;
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public string Reason { get; set; }

        public IReadOnlyDictionary<string, string> Data { get; set; }

        public TenantEvent Clone()
        {
            return (TenantEvent)this.MemberwiseClone();
        }

        public static TenantEvent Boot() { return new TenantEvent() { Type = TenantState.Boot.ToString() }; }
        public static TenantEvent Start() { return new TenantEvent() { Type = TenantState.Started.ToString() }; }
        public static TenantEvent Stop() { return new TenantEvent() { Type = TenantState.Stopped.ToString() }; }
        public static TenantEvent Shutdown() { return new TenantEvent() { Type = TenantState.Shutdown.ToString() }; }
    }
}
