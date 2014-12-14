using Microsoft.Framework.Logging;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [TenantOnly]
    public interface ITenantLogger : ILogger { }
}
