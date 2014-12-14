using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    public enum TenantState
    {
        None,
        Boot,
        Started,
        Stopped,
        Shutdown
    }
}
