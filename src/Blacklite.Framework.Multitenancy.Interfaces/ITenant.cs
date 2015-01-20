#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using Microsoft.Framework.ConfigurationModel;
using Blacklite.Framework.Events;
using System;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [TenantOnly]
    public interface ITenant : IDisposable
    {
        string Id { get; }
        IServiceProvider Services { get; }
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
