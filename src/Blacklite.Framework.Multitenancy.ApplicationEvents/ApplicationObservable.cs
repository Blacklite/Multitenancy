using Blacklite.Framework.Events;
using Blacklite.Framework.Multitenancy.Events;
using System;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationObservable : IEventObservable<IApplicationEvent>
    {
        private readonly IObservable<IApplicationEvent> _observable;
        public ApplicationObservable(IEventOrchestrator<IApplicationEvent> orchestrator)
        {
            _observable = orchestrator.Events;
        }

        public IDisposable Subscribe(IObserver<IApplicationEvent> observer)
        {
            return _observable.Subscribe(observer);
        }
    }
}
