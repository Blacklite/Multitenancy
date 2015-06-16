using Microsoft.Framework.Configuration;
using Blacklite.Framework.Events;
using System;
using System.Threading.Tasks;
using Blacklite.Framework.Multitenancy.Configuration;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy
{
    [TenantOnly]
    public interface ITenant : IDisposable
    {
        string Id { get; }
        IServiceProvider Services { get; }
        TenantState State { get; }
        TenantConfiguration Configuration { get; }
        IObservable<KeyValuePair<string, string>> ConfigurationChanged { get; }
        void Broadcast(IEvent operation);

        IObservable<IEvent> Events { get; }
        IObservable<IEvent> Boot { get; }
        IObservable<IEvent> Start { get; }
        IObservable<IEvent> Stop { get; }
        IObservable<IEvent> Shutdown { get; }

        Task DoStart();
        Task DoStop();
    }
}
