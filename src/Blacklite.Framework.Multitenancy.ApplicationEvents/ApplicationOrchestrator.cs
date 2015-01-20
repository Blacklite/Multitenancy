using Blacklite.Framework.Events;
using Blacklite.Framework.GlobalEvents;
using Blacklite.Framework.Multitenancy.ApplicationEvents;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationOrchestrator : IEventOrchestrator<IApplicationEvent>
    {
        private readonly ISubject<IApplicationEvent> _subject;

        public ApplicationOrchestrator()
        {
            _subject = new Subject<IApplicationEvent>();
            Events = _subject;
        }

        public IObservable<IApplicationEvent> Events { get; }

        public void Broadcast(IApplicationEvent value)
        {
            _subject.OnNext(value);
        }
    }
}
