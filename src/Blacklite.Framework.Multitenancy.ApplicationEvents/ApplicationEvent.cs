using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationEvent : IApplicationEvent
    {
        public ApplicationEvent()
        {

        }

        internal ApplicationEvent(string tenantId, IEvent @event)
        {
            Tenant = tenantId;
            Name = @event.Name;
            Type = @event.Type;
            User = @event.User;
            Reason = @event.Reason;
        }

        internal static ApplicationEvent Create(IEvent x) => new ApplicationEvent("_global", x);

        internal static Func<IEvent, ApplicationEvent> Create(string tenantId) => x => new ApplicationEvent(tenantId, x);

        public string Tenant { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public string Reason { get; set; }
    }
}
