using System;

namespace Blacklite.Framework.Multitenancy.Events
{
    public class Event : IEvent
    {
        public Event()
        {

        }

        internal Event(IEvent @event)
        {
            Type = @event.Type;
            Name = @event.Name;
            User = @event.User;
            Reason = @event.Reason;
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public string Reason { get; set; }

        public Event Clone()
        {
            return (Event)this.MemberwiseClone();
        }

        public static Event Boot() { return new Event() { Type = TenantState.Boot.ToString() }; }
        public static Event Start() { return new Event() { Type = TenantState.Started.ToString() }; }
        public static Event Stop() { return new Event() { Type = TenantState.Stopped.ToString() }; }
        public static Event Shutdown() { return new Event() { Type = TenantState.Shutdown.ToString() }; }
    }
}
