using Blacklite.Framework.Multitenancy.Events;
using System;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationObservable : IApplicationObservable
    {
        private readonly IObservable<IApplicationEvent> _observable;
        public ApplicationObservable(IApplicationOrchestrator orchestrator)
        {
            _observable = orchestrator.Events;
        }

        public IDisposable Subscribe(IObserver<IApplicationEvent> observer)
        {
            return _observable.Subscribe(observer);
        }
    }
}
