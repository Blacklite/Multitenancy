using Blacklite.Framework.Events;
using Blacklite.Framework.Multitenancy.Configuration;
using Blacklite.Framework.Multitenancy.Events;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Blacklite.Framework.Multitenancy
{
    public class Tenant : ITenant
    {
        private bool _initalized = false;
        private readonly Subject<IEvent> _eventObserver;
        private Task _starting;
        private Task _stopping;
        private readonly IDictionary<TenantState, TenantState[]> _validStates = new Dictionary<TenantState, TenantState[]>()
        {
            [TenantState.None] = new[] { TenantState.Boot },
            [TenantState.Boot] = new[] { TenantState.Started, TenantState.Shutdown },
            [TenantState.Started] = new[] { TenantState.Stopped },
            [TenantState.Stopped] = new[] { TenantState.Started, TenantState.Shutdown },
            [TenantState.Shutdown] = new TenantState[] { },
        };

        public Tenant(TenantConfiguration configuration)
        {
            var eventSubject = _eventObserver = new Subject<IEvent>();

            Configuration = configuration;
            ConfigurationChanged = configuration.Observable;

            Events = eventSubject;
            Boot = Events.Where(x => x.Type == TenantState.Boot.ToString());
            Boot.Subscribe(x => State = TenantState.Boot);

            Start = Events.Where(x => x.Type == TenantState.Started.ToString());
            Start.Subscribe(x => State = TenantState.Started);

            Stop = Events.Where(x => x.Type == TenantState.Stopped.ToString());
            Stop.Subscribe(x => State = TenantState.Stopped);

            Shutdown = Events.Where(x => x.Type == TenantState.Shutdown.ToString());
            Shutdown.Subscribe(x => State = TenantState.Shutdown);
        }

        public void Initialize([NotNull] string identifier, IServiceProvider serviceProvider)
        {
            if (_initalized)
                return;

            Id = identifier;
            Services = serviceProvider;
            _initalized = true;
        }

        public string Id { get; private set; }

        public TenantConfiguration Configuration { get; }

        public TenantState State { get; private set; }

        public void ChangeState(TenantState state)
        {
            Broadcast(new TenantEvent()
            {
                Type = state.ToString()
            });
        }

        /// <summary>
        /// Broadcasts from the tenant, specificly, should ignore state changes.
        ///  This allows for operations to be broadcast, that are not tenant related.
        /// </summary>
        /// <param name="event"></param>
        void ITenant.Broadcast(IEvent @event)
        {
            // Broadcasts from the tenant, specificly, should ignore state changes.
            //  This allows for operations to be broadcast, that are not tenant related.
            TenantState movingTo;
            if (Enum.TryParse(@event.Type, out movingTo))
                @event = new TenantEvent(@event) { Type = "Not" + @event.Type };

            Broadcast(@event);
        }

        public void Broadcast(IEvent @event)
        {
            TenantState movingTo;
            if (Enum.TryParse(@event.Type, out movingTo))
            {
                TenantState[] validStates;
                if (_validStates.TryGetValue(State, out validStates))
                {
                    if (validStates.Any(z => z == movingTo))
                    {
                        foreach (var s in validStates.TakeWhile(x => x != movingTo))
                        {
                            var newOperation = new TenantEvent(@event) { Type = s.ToString() };

                            _eventObserver.OnNext(newOperation);
                        }

                        _eventObserver.OnNext(@event);
                        return;
                    }
                }

                @event = new TenantEvent(@event) { Type = string.Format("InvalidStateTransition{0}", @event.Type) };
                _eventObserver.OnNext(@event);
            }
            else
            {
                _eventObserver.OnNext(@event);
            }
        }

        public void ChangeState(TenantState state, TenantEvent operation)
        {
            operation.Type = state.ToString();
            Broadcast(operation);
        }

        public IObservable<IEvent> Events { get; }
        public IObservable<IEvent> Boot { get; }
        public IObservable<IEvent> Start { get; }
        public IObservable<IEvent> Stop { get; }
        public IObservable<IEvent> Shutdown { get; }

        public IServiceProvider Services { get; private set; }

        public IObservable<KeyValuePair<string, string>> ConfigurationChanged { get; }

        public Task DoStart()
        {
            if (_starting == null && _validStates.Any(z => z.Value.Contains(TenantState.Started) && z.Key == State))
            {
                var task = _starting = new Task(() => Broadcast(new TenantEvent() { Type = TenantState.Started.ToString() }));
                task.ContinueWith(x => _starting = null);
                task.Start();
            }
            return _starting ?? Task.Delay(0);
        }

        public Task DoStop()
        {
            if (_stopping == null && _validStates.Any(z => z.Value.Contains(TenantState.Stopped) && z.Key == State))
            {
                var task = _stopping = new Task(() => Broadcast(new TenantEvent() { Type = TenantState.Stopped.ToString() }));
                task.ContinueWith(x => _stopping = null);
                task.Start();
            }
            return _stopping ?? Task.Delay(0);
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

                    _eventObserver.Dispose();
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
