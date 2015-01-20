using Blacklite.Framework.GlobalEvents;
using System;
using Blacklite.Framework.Events;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationEventSource : IGlobalEventSource
    {
        private readonly IEventObservable<IApplicationEvent> _observable;
        public ApplicationEventSource(IEventObservable<IApplicationEvent> observable)
        {
            _observable = observable;
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            return _observable.Subscribe(observer);
        }
    }
}
