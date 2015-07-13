using Blacklite.Framework.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy.Events
{
    public static class MultitenancyEvents
    {
        public static EventSource<ITenantEvent> Global = new EventSource<ITenantEvent>();

        private static readonly Subject<ITenantEvent> _subject = new Subject<ITenantEvent>();
        static MultitenancyEvents() {
            Global.Add(_subject);
        }

        public static void Broadcast(IEvent @event)
        {
            var dict = @event.Data.ToDictionary(x => x.Key, x => x.Value);
            dict[nameof(ITenantEvent.Tenant)] = "*$app$*";
            _subject.OnNext(new TenantEvent(dict));
        }
    }
}
