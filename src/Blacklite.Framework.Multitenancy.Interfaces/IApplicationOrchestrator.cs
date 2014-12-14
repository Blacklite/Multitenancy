using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface IApplicationOrchestrator
    {
        void Broadcast(IApplicationEvent value);
        IObservable<IApplicationEvent> Events { get; }
    }
}
