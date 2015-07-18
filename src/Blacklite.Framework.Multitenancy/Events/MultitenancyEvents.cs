using Blacklite.Framework.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy.Events
{
    public static class MultitenancyEvents
    {
        private static bool _addedSubject = false;
        private static readonly Subject<ITenantEvent> _subject = new Subject<ITenantEvent>();
        private static EventSource<ITenantEvent> _global = new EventSource<ITenantEvent>();
        public static EventSource<ITenantEvent> Global
        {
            get
            {
                if (!_addedSubject)
                {
                    _global.Add(_subject);
                    _addedSubject = true;
                }

                return _global;
            }
        }

        public static void Broadcast(IEvent @event)
        {
            if (!_addedSubject)
            {
                Global.Add(_subject);
            }

            var dict = @event.Data.ToDictionary(x => x.Key, x => x.Value);
            dict[nameof(ITenantEvent.Tenant)] = "*$app$*";
            _subject.OnNext(new TenantEvent(dict));
        }
    }
}
