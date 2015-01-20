#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using System;
using Blacklite.Framework.Events;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    public interface IApplicationEvent : IEvent
    {
        string Tenant { get; }
    }
}
