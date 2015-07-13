using System;
using Blacklite.Framework.Events;

namespace Blacklite.Framework.Multitenancy
{
    public interface ITenantEvent : IEvent
    {
        string Tenant { get; }
    }
}
