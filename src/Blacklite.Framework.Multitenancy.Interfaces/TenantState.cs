using System;

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
}
