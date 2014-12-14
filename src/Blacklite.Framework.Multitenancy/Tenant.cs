using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Blacklite.Framework.Multitenancy.Events;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Blacklite.Framework.Multitenancy
{
    public enum TenantState
    {
        None,
        Boot,
        Started,
        Stopped,
        Shutdown
    }

    [TenantOnly]
    public interface ITenant : IDisposable
    {
        string Id { get; }
        TenantState State { get; }
        IConfiguration Configuration { get; }
        void Broadcast(Event operation);

        IObservable<Event> Events { get; }
        IObservable<Event> Boot { get; }
        IObservable<Event> Start { get; }
        IObservable<Event> Stop { get; }
        IObservable<Event> Shutdown { get; }

        void DoStart();
        void DoStop();
    }

    public class Tenant : ITenant
    {
        private bool _initalized = false;
        private readonly IObserver<Event> _changeSubject;
        private readonly IDictionary<TenantState, TenantState[]> _validStates = new Dictionary<TenantState, TenantState[]>()
        {
            [TenantState.None] = new[] { TenantState.Boot, TenantState.Started },
            [TenantState.Boot] = new[] { TenantState.Started, TenantState.Shutdown },
            [TenantState.Started] = new[] { TenantState.Stopped, TenantState.Shutdown },
            [TenantState.Stopped] = new[] { TenantState.Started, TenantState.Shutdown },
            [TenantState.Shutdown] = new TenantState[] { },
        };

        public Tenant(ITenantConfiguration configuration)
        {
            Configuration = configuration;

            var subject = new Subject<Event>();
            _changeSubject = subject;
            Events = subject;
            Boot = Events.Where(x => x.Type == string.Format("{0}", TenantState.Boot));
            Boot.Subscribe(x => State = TenantState.Boot);

            Start = Events.Where(x => x.Type == string.Format("{0}", TenantState.Started));
            Start.Subscribe(x => State = TenantState.Started);

            Stop = Events.Where(x => x.Type == string.Format("{0}", TenantState.Stopped));
            Stop.Subscribe(x => State = TenantState.Stopped);

            Shutdown = Events.Where(x => x.Type == string.Format("{0}", TenantState.Shutdown));
            Shutdown.Subscribe(x => State = TenantState.Shutdown);
        }

        public void Initialize([NotNull] string identifier)
        {
            if (_initalized)
                return;

            Id = identifier;
            _initalized = true;
        }

        public string Id { get; private set; }

        public IConfiguration Configuration { get; }

        public TenantState State { get; private set; }

        public void ChangeState(TenantState state)
        {
            Broadcast(new Event()
            {
                Type = string.Format("{0}", state)
            });
        }

        /// <summary>
        /// Broadcasts from the tenant, specificly, should ignore state changes.
        ///  This allows for operations to be broadcast, that are not tenant related.        /// </summary>
        /// <param name="operation"></param>
        void ITenant.Broadcast(Event operation)
        {
            // Broadcasts from the tenant, specificly, should ignore state changes.
            //  This allows for operations to be broadcast, that are not tenant related.
            TenantState movingTo;
            if (Enum.TryParse(operation.Type, out movingTo))
                operation.Type = "Not" + operation.Type;

            Broadcast(operation);
        }

        public void Broadcast(Event operation)
        {
            TenantState movingTo;
            if (Enum.TryParse(operation.Type, out movingTo))
            {
                TenantState[] validStates;
                if (_validStates.TryGetValue(State, out validStates))
                {
                    if (validStates.Any(z => z == movingTo))
                    {
                        foreach (var s in validStates.TakeWhile(x => x != movingTo))
                        {
                            var newOperation = operation.Clone();
                            newOperation.Type = string.Format("{0}", s);

                            _changeSubject.OnNext(newOperation);
                        }

                        _changeSubject.OnNext(operation);
                        return;
                    }
                }

                operation.Type = string.Format("InvalidStateTransition{0}", operation.Type);
                _changeSubject.OnNext(operation);
            }
            else
            {
                _changeSubject.OnNext(operation);
            }
        }

        public void ChangeState(TenantState state, Event operation)
        {
            operation.Type = string.Format("{0}", state);
            Broadcast(operation);
        }

        public IObservable<Event> Events { get; }
        public IObservable<Event> Boot { get; }
        public IObservable<Event> Start { get; }
        public IObservable<Event> Stop { get; }
        public IObservable<Event> Shutdown { get; }

        public void DoStart()
        {
            Broadcast(new Event() { Type = string.Format("{0}", TenantState.Started) });
        }

        public void DoStop()
        {
            Broadcast(new Event() { Type = string.Format("{0}", TenantState.Stopped) });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (State == TenantState.Started)
                    {
                        ChangeState(TenantState.Stopped);
                    }

                    if (State == TenantState.Stopped)
                    {
                        ChangeState(TenantState.Shutdown);
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
