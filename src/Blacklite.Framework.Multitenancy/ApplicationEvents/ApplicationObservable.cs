using Blacklite.Framework.Multitenancy.Events;
using System;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public interface IApplicationObservable : IObservable<ApplicationEvent>
    {

    }

    public class ApplicationObservable : IApplicationObservable
    {
        private readonly IObservable<ApplicationEvent> _observable;
        public ApplicationObservable(IApplicationOrchestrator orchestrator)
        {
            _observable = orchestrator.Events;
        }

        public IDisposable Subscribe(IObserver<ApplicationEvent> observer)
        {
            return _observable.Subscribe(observer);
        }
    }
}
