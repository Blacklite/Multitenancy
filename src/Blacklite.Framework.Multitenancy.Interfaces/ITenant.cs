using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [TenantOnly]
    public interface ITenant : IDisposable
    {
        string Id { get; }
        TenantState State { get; }
        IConfiguration Configuration { get; }
        void Broadcast(IEvent operation);

        IObservable<IEvent> Events { get; }
        IObservable<IEvent> Boot { get; }
        IObservable<IEvent> Start { get; }
        IObservable<IEvent> Stop { get; }
        IObservable<IEvent> Shutdown { get; }

        void DoStart();
        void DoStop();
    }
}
