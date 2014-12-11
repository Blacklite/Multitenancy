using Blacklite.Framework.Multitenancy.Events;
using System;
using System.Reactive;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public interface IApplicationOrchestrator
    {
        void Broadcast(ApplicationEvent value);
        IObservable<ApplicationEvent> Events { get; }
    }

    public class ApplicationOrchestrator : IApplicationOrchestrator
    {
        private readonly ISubject<ApplicationEvent> _subject;

        public ApplicationOrchestrator()
        {
            _subject = new Subject<ApplicationEvent>();
            Events = _subject;
        }

        public IObservable<ApplicationEvent> Events { get; }

        public void Broadcast(ApplicationEvent value)
        {
            _subject.OnNext(value);
        }
    }
}
