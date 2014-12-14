using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    public interface IApplicationEvent : IEvent
    {
        string Tenant { get; }
    }
}
