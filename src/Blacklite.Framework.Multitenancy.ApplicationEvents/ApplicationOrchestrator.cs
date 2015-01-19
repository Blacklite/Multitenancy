using Blacklite.Framework.GlobalEvents;
using Blacklite.Framework.Multitenancy.ApplicationEvents;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationOrchestrator : IApplicationOrchestrator
    {
        private readonly ISubject<IApplicationEvent> _subject;

        public ApplicationOrchestrator(IGlobalObservable globalObservable)
        {
            _subject = new Subject<IApplicationEvent>();
            Events = _subject;

            globalObservable
                .Select(ApplicationEvent.Create)
                .Subscribe(_subject.OnNext);
        }

        public IObservable<IApplicationEvent> Events { get; }

        public void Broadcast(IApplicationEvent value)
        {
            _subject.OnNext(value);
        }
    }
}
