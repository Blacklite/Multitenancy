using System;
using Blacklite.Framework.Events;

namespace Blacklite.Framework.Multitenancy
{
    public interface IApplicationEvent : IEvent
    {
        string Tenant { get; }
    }
}
