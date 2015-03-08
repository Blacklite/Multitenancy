using System;

namespace Blacklite.Framework.Multitenancy
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class TenantOnlyAttribute : Attribute { }
}
