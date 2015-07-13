using System;

namespace Blacklite.Framework.Multitenancy.Http
{
    public class TenantMiddlewareOptions
    {
        public bool ThrowWhenFailed { get; set; }
        public bool ThrowWhenOffline { get; set; }
    }
}
