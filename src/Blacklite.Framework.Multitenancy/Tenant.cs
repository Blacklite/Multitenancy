using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Blacklite.Framework.Multitenancy.Operations;
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

    public interface ITenant : IDisposable
    {
        void Initialize([NotNull] string identifier);
        string Id { get; }
        TenantState State { get; }
        IConfiguration Configuration { get; }

        IObservable<Operation> Change { get; }
        IObservable<Operation> Boot { get; }
        IObservable<Operation> Start { get; }
        IObservable<Operation> Stop { get; }
        IObservable<Operation> Shutdown { get; }
    }

    public class Tenant : ITenant
    {
        private bool _initalized = false;
        private readonly IObserver<Operation> _changeSubject;
        private readonly IDictionary<TenantState, TenantState[]> _validStates = new Dictionary<TenantState, TenantState[]>()
        {
            [TenantState.None] = new[] { TenantState.Boot, TenantState.Started },
            [TenantState.Boot] = new[] { TenantState.Started },
            [TenantState.Started] = new[] { TenantState.Stopped, TenantState.Shutdown },
            [TenantState.Stopped] = new[] { TenantState.Shutdown },
            [TenantState.Shutdown] = new TenantState[] { },
        };

        public Tenant(ITenantConfiguration configuration)
        {
            Configuration = configuration;

            var subject = new Subject<Operation>();
            _changeSubject = subject;
            Change = subject;
            Boot = Change.Where(x => x.Type == "\{TenantState.Boot}");
            Start = Change.Where(x => x.Type == "\{TenantState.Started}");
            Stop = Change.Where(x => x.Type == "\{TenantState.Stopped}");
            Shutdown = Change.Where(x => x.Type == "\{TenantState.Shutdown}");


            Boot.Subscribe(x => State = TenantState.Boot);
            Start.Subscribe(x => State = TenantState.Started);
            Stop.Subscribe(x => State = TenantState.Stopped);
            Shutdown.Subscribe(x => State = TenantState.Shutdown);
        }

        public void Initialize(string identifier)
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
            ChangeState(new Operation()
            {
                Type = "\{state}"
            });
        }

        public void ChangeState(Operation operation)
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
                            newOperation.Type = "\{s}";

                            _changeSubject.OnNext(newOperation);
                        }

                        _changeSubject.OnNext(operation);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            _changeSubject.OnNext(operation);
        }

        public void ChangeState(TenantState state, Operation operation)
        {
            operation.Type = "\{state}";
            ChangeState(operation);
        }

        public IObservable<Operation> Change { get; }
        public IObservable<Operation> Boot { get; }
        public IObservable<Operation> Start { get; }
        public IObservable<Operation> Stop { get; }
        public IObservable<Operation> Shutdown { get; }

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