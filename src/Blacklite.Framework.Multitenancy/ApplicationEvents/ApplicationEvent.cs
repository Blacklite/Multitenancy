using Blacklite.Framework.Multitenancy.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationEvent : Event
    {
        public static Func<Event, ApplicationEvent> Create(string tenantId)
        {
            return operation => new ApplicationEvent()
            {
                Tenant = tenantId,
                Name = operation.Name,
                Type = operation.Type
            };
        }

        public string Tenant { get; set; }
    }
}
